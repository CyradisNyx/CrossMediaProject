using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour
{
    public Text turnText;
    public int currentTurn = 0;
    public int maxTurn = 5;

    public void Start()
    {
        if (turnText == null)
        {
            turnText = transform.GetComponentInChildren<Text>();
        }
        turnText.text = $"Turn {currentTurn}/{maxTurn}";

        EventMaster.Instance.ONEndTurn += OnEndTurn;
    }

    public void OnEndTurn()
    {
        currentTurn++;
        turnText.text = $"Turn {currentTurn}/{maxTurn}";
        if (currentTurn == maxTurn)
        {
            EventMaster.Instance.FailLevel();
        }
    }
}