using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public int completedLevel;
    public Dictionary<int, bool> coinsCollected;
    public int money;

    public ProgressData(int level = 0, int money = 0)
    {
        completedLevel = level;
        this.money = money;
        coinsCollected = new Dictionary<int, bool>();
    }
}