using System;
using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    public ProgressComponent progressComponent;
    public bool collected = false;
    public int coinValue = 100;

    public void Start()
    {
        if (progressComponent == null)
        {
            progressComponent = GameObject.Find("PlayerProgress").GetComponent<ProgressComponent>();
        }

        if (progressComponent.CoinsCollected)
        {
            this.gameObject.SetActive(false);
            return;
        }

        EventMaster.Instance.ONCompleteLevel += OnCompleteLevel;
    }

    public void OnCompleteLevel()
    {
        progressComponent.Earn(coinValue);
    }
}