using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EndLevelDialog : MonoBehaviour
    {
        public void Start()
        {
            EventMaster.Instance.ONCompleteLevel += OnCompleteLevel;
            gameObject.SetActive(false);
        }

        public void OnCompleteLevel()
        {
            gameObject.SetActive(true);
        }
    }
}