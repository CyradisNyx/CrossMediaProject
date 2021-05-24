using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HumanController : MonoBehaviour
{
    public List<GameObject> cutterObjects;
    public float affectRange = 1.5f;
    public float lookRange = 1.5f;
    public TileBase forestTile;
    public TileBase grassTile;
    public TileBase houseTile;

    public void Start()
    {
        cutterObjects = new List<GameObject>();
        foreach (Transform child in transform)
        {
            cutterObjects.Add(child.gameObject);
        }
    }

    public bool isBlocked(Vector3 pos)
    {
        foreach (var cutterObject in cutterObjects)
        {
            if (cutterObject.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }
}
