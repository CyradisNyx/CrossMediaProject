using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public PlayerController player;
    public GameObject highlightPrefab;
    public List<TileData> tileData;
    
    private Grid _grid;
    private Tilemap _tilemap;
    private List<GameObject> _highlightedTiles;
    private Dictionary<TileBase, TileData> _dataFromTiles;
    private FireController _fireController;
    private HumanController _humanController;

    private void Start()
    {
        _grid = gameObject.GetComponent<Grid>();
        _tilemap = gameObject.GetComponentInChildren<Tilemap>();
        _highlightedTiles = new List<GameObject>();
        _dataFromTiles = new Dictionary<TileBase, TileData>();
        _fireController = gameObject.GetComponentInChildren<FireController>();
        _humanController = gameObject.GetComponentInChildren<HumanController>();

        foreach (var data in tileData)
        {
            foreach (var tile in data.tiles)
            {
                _dataFromTiles.Add(tile, data);
            }
        }

        EventMaster.Instance.ONSelectPlayer += OnSelectPlayer;
        EventMaster.Instance.ONDeselectPlayer += OnDeselectPlayer;
        EventMaster.Instance.ONStartTurn += OnStartTurn;
        EventMaster.Instance.ONEndTurn += OnEndTurn;
        
        OnStartTurn();
    }

    public void OnDestroy()
    {
        EventMaster.Instance.ONSelectPlayer -= OnSelectPlayer;
        EventMaster.Instance.ONDeselectPlayer -= OnDeselectPlayer;
        EventMaster.Instance.ONStartTurn -= OnStartTurn;
        EventMaster.Instance.ONEndTurn -= OnEndTurn;
    }

    public void OnStartTurn()
    {
        List<Vector3Int> cells = GenerateRange(GetGridPosition(player.startPoint.gameObject));
        cells = CullMovementViability(cells);
        foreach (var cell in cells)
        {
            CreateHighlighted(cell);
        }

        if (_fireController)
        {
            KillFire();
            SpreadFire();
            CreateChar();
        }

        if (_humanController)
        {
            AffectCurrent();
            Move();
        }
    }

    public void OnEndTurn()
    {
        DestroyHighlighted();
    }

    public void OnSelectPlayer()
    {
        EnableHighlighted();
    }

    public void OnDeselectPlayer()
    {
        DisableHighlighted();
    }

    private void AffectCurrent()
    {
        foreach (var o in _humanController.cutterObjects)
        {
            Vector3Int gridPos = GetGridPosition(o);
            TileBase currentTile = _tilemap.GetTile(gridPos);
            if (currentTile == _humanController.forestTile)
            {
                _tilemap.SetTile(gridPos, _humanController.grassTile);
            }

            if (currentTile == _humanController.grassTile)
            {
                _tilemap.SetTile(gridPos, _humanController.houseTile);
            }
        }
    }

    private void Move()
    {
        foreach (var o in _humanController.cutterObjects)
        {
            Vector3Int gridPos = GetGridPosition(o);
            TileBase currentTile = _tilemap.GetTile(gridPos);
            if (currentTile == _humanController.forestTile || currentTile == _humanController.grassTile)
            {
                continue;
            }
            // Generate and shuffle possible new spots
            List<Vector3Int> surrounds = GenerateRange(gridPos, _humanController.lookRange);
            surrounds = surrounds.OrderBy(x => Guid.NewGuid()).ToList();
            foreach (var i in surrounds)
            {
                TileBase possibleTile = _tilemap.GetTile(i);
                if (
                    possibleTile == _humanController.forestTile ||
                    possibleTile == _humanController.grassTile
                    )
                {
                    o.transform.position = _grid.CellToWorld(i);
                    break;
                }
            }
        }
    }

    private void KillFire()
    {
        List<GameObject> toKill = new List<GameObject>();
        
        foreach (var o in _fireController.flameObjects)
        {
            Vector3Int gridPos = GetGridPosition(o);
            List<Vector3Int> surrounds = GenerateRange(gridPos, _fireController.flameRange);
            
            // Check if all surrounds are flammable
            bool flammable = false;
            foreach (var i in surrounds)
            {
                TileBase currentTile = _tilemap.GetTile(i);

                if (_dataFromTiles[currentTile].flammable)
                {
                    flammable = true;
                }
            }
            
            if (!flammable)
            {
                toKill.Add(o);
            }
        }

        foreach (var o in toKill)
        {
            _fireController.flameObjects.Remove(o);
            Destroy(o);
        }
    }

    private void SpreadFire()
    {
        List<Vector3Int> flameSpots = new List<Vector3Int>();
        foreach (var flameObject in _fireController.flameObjects)
        {
            Vector3Int gridPos = GetGridPosition(flameObject);
            flameSpots.AddRange(GenerateRange(gridPos, _fireController.flameRange));
        }
        
        // Cull Flames
        flameSpots = flameSpots.Distinct().ToList();
        flameSpots = CullFire(flameSpots);
        
        // Create new flames
        foreach (var i in flameSpots)
        {
            CreateFlame(i);
        }
    }

    private void CreateFlame(Vector3Int gridPos)
    {
        Vector3 worldPos = _grid.CellToWorld(gridPos);
        GameObject flamePrefab = _fireController.flameObjects[0];
        GameObject obj = Instantiate(flamePrefab, worldPos, Quaternion.identity);
        obj.transform.parent = _fireController.transform;
        _fireController.flameObjects.Add(obj);
    }

    private List<Vector3Int> CullFire(List<Vector3Int> flameSpots)
    {
        List<Vector3Int> alreadyAlight = new List<Vector3Int>();
        foreach (var o in _fireController.flameObjects)
        {
            alreadyAlight.Add(GetGridPosition(o));
        }
        
        List<Vector3Int> flammable = new List<Vector3Int>();
        foreach (var i in flameSpots)
        {
            TileBase currentTile = _tilemap.GetTile(i);

            // Check if tile can be set on fire
            if (_dataFromTiles[currentTile].flammable)
            {
                // Check if tile is already on fire
                if (!alreadyAlight.Contains(i))
                {
                    flammable.Add(i);
                }
            }
        }

        return flammable;
    }

    private void CreateChar()
    {
        foreach (var flameObject in _fireController.flameObjects)
        {
            Vector3Int gridPos = GetGridPosition(flameObject);
            TileBase currentTile = _tilemap.GetTile(gridPos);
            if (currentTile.name != "char")
            {
                _tilemap.SetTile(gridPos, _fireController.charTile);
            }
        }
    }

    private List<Vector3Int> CullMovementViability(List<Vector3Int> movementRange)
    {
        List<Vector3Int> results = new List<Vector3Int>();
        foreach (var i in movementRange)
        {
            TileBase currentTile = _tilemap.GetTile(i);

            if (!_dataFromTiles[currentTile].navigable)
            {
                continue;
            }

            if (_humanController)
            {
                if (_humanController.isBlocked(_grid.CellToWorld(i)))
                {
                    continue;
                }
            }

            if (_fireController)
            {
                if (_fireController.isBlocked(_grid.CellToWorld(i)))
                {
                    continue;
                }
            }

            results.Add(i);
        }
        Debug.Log(results);
        return results;
    }

    private List<Vector3Int> GenerateRange(Vector3Int centerPos, float range = 0)
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        TileBase currentTile = _tilemap.GetTile(centerPos);
        if (range <= 0)
        {
            range = _dataFromTiles[currentTile].movementRange;
        }

        int left = (int)(centerPos.x - range - 1);
        int right = (int)(centerPos.x + range + 1);
        int bottom = (int)(centerPos.y - range - 1);
        int top = (int)(centerPos.y + range + 1);

        for (int x = left; x <= right; x++)
        {
            for (int y = bottom; y <= top; y++)
            {
                Vector3Int checkTile = new Vector3Int(x, y, centerPos.z);
                if (InsideRange(centerPos, checkTile, range))
                {
                    cells.Add(checkTile);
                }
            }
        }
        return cells;
    }

    private bool InsideRange(Vector3Int center, Vector3Int checkTile, float radius)
    {
        float dx = center.x - checkTile.x;
        float dy = center.y - checkTile.y;
        float distanceSquared = dx * dx + dy * dy;
        return distanceSquared <= radius * radius;
    }

    void CreateHighlighted(Vector3Int cell)
    {
        Vector3 worldPos = _grid.CellToWorld(cell);
        GameObject obj = Instantiate(highlightPrefab, worldPos, Quaternion.identity);
        obj.SetActive(false);
        _highlightedTiles.Add(obj);
    }
    
    void DestroyHighlighted()
    {
        foreach (var highlightedTile in _highlightedTiles)
        {
            Destroy(highlightedTile.gameObject);
        }
        _highlightedTiles.Clear();
    }

    void EnableHighlighted()
    {
        foreach (var highlightedTile in _highlightedTiles)
        {
            highlightedTile.SetActive(true);
        }
    }

    void DisableHighlighted()
    {
        foreach (var highlightedTile in _highlightedTiles)
        {
            highlightedTile.SetActive(false);
        }
    }

    Vector3Int GetGridPosition(GameObject obj)
    {
        return _grid.WorldToCell(obj.transform.position);
    }
}
