using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour
{
    public ProgressComponent progress;
    public Text turnText;
    public int currentTurn = 0;
    public int maxTurn = 5;

    public void Start()
    {
        if (turnText == null)
        {
            turnText = transform.GetComponentInChildren<Text>();
        }
        if (progress == null)
        {
            progress = GameObject.Find("PlayerProgress").GetComponent<ProgressComponent>();
        }

        maxTurn = progress.maxTurns;
        turnText.text = $"Turn {currentTurn}/{maxTurn}";

        EventMaster.Instance.ONEndTurn += OnEndTurn;
    }

    public void OnDestroy()
    {
        EventMaster.Instance.ONEndTurn -= OnEndTurn;
    }

    public void OnEndTurn()
    {
        currentTurn++;
        turnText.text = $"Turn {currentTurn}/{maxTurn}";
        if (currentTurn > maxTurn)
        {
            Debug.Log("Failed Level in TurnTimer");
            EventMaster.Instance.FailLevel();
        }
    }
}