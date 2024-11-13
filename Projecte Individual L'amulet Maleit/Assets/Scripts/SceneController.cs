using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    private static void LoadScene(int sceneIndex)
    {
        if (sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public static void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static int GetActualSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // Diferents Loads
    public static void LoadMainMenuScene()
    {
        LoadScene(0);
    }
    public static void LoadMainWorldScene()
    {
        LoadScene(1);
    }
    public static void LoadBattleScene()
    {
        LoadScene(2);
    }
    public static void LoadForestScene()
    {
        LoadScene(3);
    }
    public static void LoadCaveScene()
    {
        LoadScene(4);
    }
    public static void LoadWinScene()
    {
        LoadScene(5);
    }
    public static void LoadLoseScene()
    {
        LoadScene(6);
    }

    public static void LoadSceneByOrder(int order)
    {
        switch (order)
        {
            case 0:
                LoadMainMenuScene();
                break;
            case 1: 
                LoadMainWorldScene();
                break;
            case 2: 
                LoadBattleScene();
                break;
            case 3: 
                LoadForestScene();
                break;
            case 4: 
                LoadCaveScene();
                break;
            case 5: 
                LoadWinScene();
                break;
            case 6: 
                LoadLoseScene();
                break;
        }
    }
}
