using System;
using UnityEngine;

public class HighlightedTile : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("CLICKED TILE");
        EventMaster.Instance.HighlightedTileClicked(this.gameObject);
    }
}