using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMaster : MonoBehaviour
{
    private static EventMaster _instance;

    public static EventMaster Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    public event Action ONSelectPlayer;
    public void SelectPlayer()
    {
        ONSelectPlayer?.Invoke();
    }
    
    public event Action ONDeselectPlayer;
    public void DeselectPlayer()
    {
        ONDeselectPlayer?.Invoke();
    }

    public event Action<GameObject> ONHighlightedTileClicked;
    public void HighlightedTileClicked(GameObject which)
    {
        ONHighlightedTileClicked?.Invoke(which);   
    }

    public event Action ONStartTurn;
    public void StartTurn()
    {
        ONStartTurn?.Invoke();
    }
    
    public event Action ONEndTurn;
    public void EndTurn()
    {
        ONEndTurn?.Invoke();
        StartTurn();
    }
}