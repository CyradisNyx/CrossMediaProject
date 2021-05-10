using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarChecker : MonoBehaviour
{
    public int levelID;
    public Image coverImage;
    public GameObject cloudCover;
    public ProgressComponent progressComponent;

    private bool revealing;
    private float revealSpeed = 0.8f;
    private float cullDistance;

    void Start()
    {
        // Check if ID is unlocked in save file
        if (progressComponent == null)
        {
            progressComponent = GameObject.Find("PlayerProgress").GetComponent<ProgressComponent>();
        }

        // If unlocked and completed, remove cover object
        if (progressComponent.LastCompletedLevel >= levelID)
        {
            revealing = true;
        }
        // If locked, disable self
        else if (progressComponent.LastCompletedLevel < levelID - 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (revealing) {Reveal();}
    }

    public void Reveal()
    {
        revealing = true;
        if (coverImage.color.a <= 0)
        {
            revealing = false;
            return;
        }
        else
        {
            Color originalColor = coverImage.color;
            float tempAlpha = originalColor.a;
            tempAlpha -= revealSpeed * Time.deltaTime;
            coverImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, tempAlpha);
        }
    }
}
