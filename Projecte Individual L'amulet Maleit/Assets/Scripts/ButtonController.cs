using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private bool isGamePaused;
    private int[] pausableScenes;
    [SerializeField] GameObject pauseMenuCanvas;
    private void Start()
    {
        pausableScenes = new int[] { 1, 2 };
        isGamePaused = false;
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(isGamePaused);
        }
        InputController.OnPauseGameInput += PauseMenu;
    }
    private void OnDestroy()
    {
        InputController.OnPauseGameInput -= PauseMenu;
    }


    public void RestartGame() // tot el joc
    {
        SceneController.LoadScene(0);
    }

    public void StartGame() // mainWorld
    {
        SceneController.LoadScene(1);
    }

    public void ExitGame()
    {
        ClosePauseMenu();
        SceneController.ExitGame();
    }

    private void OpenPauseMenu()
    {
        isGamePaused = true;
        pauseMenuCanvas.SetActive(isGamePaused);
        Time.timeScale = 0.0f;
    }

    private void ClosePauseMenu()
    {
        isGamePaused = false;
        pauseMenuCanvas.SetActive(isGamePaused);
        Time.timeScale = 1.0f;
    }

    public void PauseMenu()
    {
        if (pausableScenes.Contains(SceneController.GetActualSceneIndex())) // comprovar si es pot pausar l'escena
        {
            if (!isGamePaused)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }
    }

    public void OpenConfig()
    {
        Debug.Log("Se ha abierto la configuración, pero aun no hay nada que configurar!");
    }
}
