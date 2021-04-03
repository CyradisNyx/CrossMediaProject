using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public PlayerController player;
    public GameObject highlightPrefab;

    private Grid grid;
    private List<GameObject> highlightedTiles;

    private void Start()
    {
        grid = gameObject.GetComponent<Grid>();
        highlightedTiles = new List<GameObject>();
    }

    private void Update()
    {
        ClearHighlighted();
        HighlightTile(GetPlayerPosition());
    }

    void ClearHighlighted()
    {
        foreach (var highlightedTile in highlightedTiles)
        {
            Destroy(highlightedTile);
        }
        highlightedTiles.Clear();
    }

    void HighlightTile(Vector3Int cell)
    {
        Vector3 worldPos = grid.CellToWorld(cell);
        GameObject obj = Instantiate(highlightPrefab, worldPos, Quaternion.identity);
        highlightedTiles.Add(obj);
    }

    Vector3Int GetPlayerPosition()
    {
        return grid.WorldToCell(player.transform.position);
    }
}
