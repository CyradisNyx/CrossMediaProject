using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireController : MonoBehaviour
{
    public List<GameObject> flameObjects;
    public TileBase charTile;
    public float flameRange = 1.5f;

    public void Start()
    {
        flameObjects = new List<GameObject>();
        foreach (Transform child in transform)
        {
            flameObjects.Add(child.gameObject);
        }
    }
    
    public bool isBlocked(Vector3 pos)
    {
        foreach (var flameObject in flameObjects)
        {
            if (flameObject.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }
}