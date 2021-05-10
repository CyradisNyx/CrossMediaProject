using System;
using UnityEngine;


public class EndLevelDialog : MonoBehaviour
{
    public GameObject complete;
    public GameObject fail;
    
    public void Start()
    {
        if (complete == null)
        {
            complete = transform.Find("CompleteLevel").gameObject;
        }
        if (fail == null)
        {
            fail = transform.Find("FailLevel").gameObject;
        }
        
        EventMaster.Instance.ONCompleteLevel += OnCompleteLevel;
        EventMaster.Instance.ONFailLevel += OnFailLevel;
        complete.SetActive(false);
        fail.SetActive(false);
    }

    public void OnCompleteLevel()
    {
        complete.SetActive(true);
    }

    public void OnFailLevel()
    {
        fail.SetActive(true);
    }
}