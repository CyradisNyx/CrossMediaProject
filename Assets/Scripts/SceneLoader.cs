using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
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
}
