using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    public void EndTurn()
    {
        if (Time.timeScale != 0)
        {
            EventMaster.Instance.EndTurn();
        }
    }
}
