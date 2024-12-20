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
            if (CanvasFade.Instance != null)
            {
                //CanvasFade.Instance.FadeCanvas();
                CanvasFade.Instance.LoadSceneFadingCoroutine(sceneIndex);
            }
            //StartCoroutine(LoadSceneFading(sceneIndex)); NO ES POT FER AQUI PERQU� ES EST�TIC!
            //SceneManager.LoadScene(sceneIndex);
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


    // Diferents Loads per cada escena
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
        RuntimeGameSettings.Instance.SetLastScene(GetActualSceneIndex());
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
        SceneManager.LoadScene(5);
        //LoadScene(5);
    }
    public static void LoadLoseScene()
    {
        LoadScene(6);
    }

    public static void LoadSceneByIndex(int index)
    {
        switch (index)
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

    public static int GetMainMenuIndex() { return 0; }
    public static int GetMainWorldIndex() { return 1; }
    public static int GetBattleIndex() { return 2; }
    public static int GetForestIndex() { return 3; }
    public static int GetCaveIndex() { return 4; }
    public static int GetWinIndex() { return 5; }
    public static int GetLoseIndex() { return 6; }
}
