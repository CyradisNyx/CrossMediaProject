using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public ProgressComponent LevelInfo;
    public void Start()
    {
        if (!LevelInfo)
        {
            this.LevelInfo = GameObject.Find("PlayerProgress").GetComponent<ProgressComponent>();
        }
    }

    public void LoadStoryMap()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StoryMap");
    }

    public void LoadEndless()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Endless");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel(string levelNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_" + levelNumber);
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_" + LevelInfo.thisLevel);
    }

    public void LoadNext()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_" + (LevelInfo.thisLevel + 1));
    }
}
