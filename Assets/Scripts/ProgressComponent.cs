﻿using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ProgressComponent : MonoBehaviour
{
    private ProgressData data;
    public int thisLevel = 0;
    public int maxTurns = 5;
    public bool resetOnLoad = false;

    public int LastCompletedLevel => data.completedLevel;
    public int CurrentMoney => data.money;

    public bool CoinsCollected
    {
        get
        {
            if (data.coinsCollected.ContainsKey(thisLevel))
            {
                return data.coinsCollected[thisLevel];
            }
            else
            {
                //data.coinsCollected.Add(thisLevel, false);
                return false;
            }
        }
    }

    public void Awake()
    {
        if (resetOnLoad)
        {
            SaveSystem.ClearProgress();
        }
        
        data = SaveSystem.LoadProgress();
        Debug.Log(LastCompletedLevel);
    }

    public void Start()
    {
        EventMaster.Instance.ONCompleteLevel += OnCompleteLevel;
    }

    public void OnDestroy()
    {
        EventMaster.Instance.ONCompleteLevel -= OnCompleteLevel;
    }

    public void Spend(int howMuch)
    {
        data.money -= howMuch;
        UpdateSaveData();
    }

    public void Earn(int howMuch)
    {
        data.money += howMuch;
        data.coinsCollected.Add(thisLevel, true);
        UpdateSaveData();
    }

    public void OnCompleteLevel()
    {
        Debug.Log("Complete!!!");
        data.completedLevel = Math.Max(thisLevel, data.completedLevel);
        UpdateSaveData();
        Time.timeScale = 0;
    }

    public void UpdateSaveData()
    {
        SaveSystem.SaveProgress(data);
    }
}