using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ProgressComponent : MonoBehaviour
{
    private ProgressData data;

    public int LastCompletedLevel => data.completedLevel;
    public int CurrentMoney => data.money;

    public void Awake()
    {
        data = SaveSystem.LoadProgress();
    }

    public void Spend(int howMuch)
    {
        data.money -= howMuch;
        UpdateSaveData();
    }

    public void Earn(int howMuch)
    {
        data.money += howMuch;
        UpdateSaveData();
    }

    public void Complete(int level)
    {
        data.completedLevel = level;
        UpdateSaveData();
    }

    public void UpdateSaveData()
    {
        SaveSystem.SaveProgress(data);
    }
}