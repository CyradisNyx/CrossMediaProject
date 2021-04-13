using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ProgressComponent : MonoBehaviour
{
    private ProgressData data;
    public int thisLevel = 0;

    public int LastCompletedLevel => data.completedLevel;
    public int CurrentMoney => data.money;

    public void Awake()
    {
        data = SaveSystem.LoadProgress();

        EventMaster.Instance.ONCompleteLevel += Complete;
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

    public void Complete()
    {
        data.completedLevel = thisLevel;
        UpdateSaveData();
        Time.timeScale = 0;
    }

    public void UpdateSaveData()
    {
        SaveSystem.SaveProgress(data);
    }
}