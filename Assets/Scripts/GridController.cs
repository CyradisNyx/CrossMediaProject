using System;
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

    private void Start()
    {
        _grid = gameObject.GetComponent<Grid>();
        _tilemap = gameObject.GetComponentInChildren<Tilemap>();
        _highlightedTiles = new List<GameObject>();
        _dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var data in tileData)
        {
            foreach (var tile in data.tiles)
            {
                _dataFromTiles.Add(tile, data);
            }
        }

        EventMaster.Instance.ONSelectPlayer += OnSelectPlayer;
        EventMaster.Instance.ONDeselectPlayer += OnDeselectPlayer;
    }

    public void OnSelectPlayer()
    {
        List<Vector3Int> cells = GenerateMovementRange();
        foreach (var cell in cells)
        {
            HighlightTile(cell);
        }
    }

    public void OnDeselectPlayer()
    {
        ClearHighlighted();
    }

    private List<Vector3Int> GenerateMovementRange()
    {
        List<Vector3Int> cells = new List<Vector3Int>();
        Vector3Int currentPosition = GetPlayerPosition();
        
        cells.Add(currentPosition);
        TileBase currentTile = _tilemap.GetTile(currentPosition);
        int range = _dataFromTiles[currentTile].movementRange;

        for (int i = 1; i <= range; i++)
        {
            cells.Add(new Vector3Int(currentPosition.x - i, currentPosition.y, currentPosition.z)); // left
            cells.Add(new Vector3Int(currentPosition.x + i, currentPosition.y, currentPosition.z)); // right
            cells.Add(new Vector3Int(currentPosition.x, currentPosition.y - i, currentPosition.z)); // down
            cells.Add(new Vector3Int(currentPosition.x, currentPosition.y + i, currentPosition.z)); // up
        }

        return cells;
    }

    void ClearHighlighted()
    {
        foreach (var highlightedTile in _highlightedTiles)
        {
            Destroy(highlightedTile);
        }
        _highlightedTiles.Clear();
    }

    void HighlightTile(Vector3Int cell)
    {
        Vector3 worldPos = _grid.CellToWorld(cell);
        GameObject obj = Instantiate(highlightPrefab, worldPos, Quaternion.identity);
        _highlightedTiles.Add(obj);
    }

    Vector3Int GetPlayerPosition()
    {
        return _grid.WorldToCell(player.startPoint.position);
    }
}
