﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        _grid = gameObject.GetComponent<Grid>();
        _tilemap = gameObject.GetComponentInChildren<Tilemap>();
        _highlightedTiles = new List<GameObject>();
        _dataFromTiles = new Dictionary<TileBase, TileData>();
        _fireController = gameObject.GetComponentInChildren<FireController>();

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
            SpreadFire();
            CreateChar();
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

    private void SpreadFire()
    {
        foreach (var flameObject in _fireController.flameObjects)
        {
            Vector3Int gridPos = _grid.WorldToCell(flameObject.transform.position);
        }
    }

    private void CreateChar()
    {
        foreach (var flameObject in _fireController.flameObjects)
        {
            Vector3Int gridPos = _grid.WorldToCell(flameObject.transform.position);
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

            if (_dataFromTiles[currentTile].navigable)
            {
                results.Add(i);
            }
        }

        return results;
    }

    private List<Vector3Int> GenerateRange(Vector3Int centerPos)
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        TileBase currentTile = _tilemap.GetTile(centerPos);
        float range = _dataFromTiles[currentTile].movementRange;

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
