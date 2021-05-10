using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStoryMap()
    {
        SceneManager.LoadScene("StoryMap");
    }

    public void LoadEndless()
    {
        SceneManager.LoadScene("Endless");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel(string levelNumber)
    {
        Debug.Log("Loading...");
        SceneManager.LoadScene("Level_" + levelNumber);
    }
}
