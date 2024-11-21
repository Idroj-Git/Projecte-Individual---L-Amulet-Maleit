using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private bool isGamePaused;
    private int[] pausableScenes;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] CanvasGroup pauseMenuCanvasGroup;
    private void Start()
    {
        pausableScenes = new int[] { 1, 2, 3, 4};
        isGamePaused = false;

        if (pausableScenes.Contains(SceneController.GetActualSceneIndex()) && pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(isGamePaused);
        }

        if (PlayerPrefs.GetInt("GameStarted")  == 0)
        {
            PlayerPrefs.SetInt("StoryFlag", 1); // Reset de la historia
        }
    }

    private void OnEnable()
    {
        InputController.OnPauseGameInput += PauseMenu; // Quan s'apreta ESC
    }

    private void OnDisable()
    {
        InputController.OnPauseGameInput -= PauseMenu;
    }


    public void RestartGame() // tot el joc
    {
        if (isGamePaused)
        {
            ClosePauseMenu();
        }

        SceneController.LoadMainMenuScene();
    }

    public void StartGame() // mainWorld
    {
        PlayerPrefs.SetInt("GameStarted", 1);
        SceneController.LoadMainWorldScene();
    }

    public void ExitGame() //apagar el joc
    {
        SceneController.ExitGame();
    }
    public void OpenConfig()
    {
        Debug.Log("Se ha abierto la configuración, pero aun no hay nada que configurar!");
    }


    //*******************      PAUSE MENU    **********************//
    private void OpenPauseMenu()
    {
        isGamePaused = true;
        pauseMenuCanvas.SetActive(isGamePaused);
        pauseMenuCanvasGroup.alpha = 0f; // Posar en el inspector (prefab) el canvasGroup.alpha = 0, quan ho tingui acabat
        FadeIn();
        Time.timeScale = 0.0f;
    }

    private void ClosePauseMenu()
    {
        isGamePaused = false;
        FadeOut();
        Time.timeScale = 1.0f;
    }

    public void PauseMenu()
    {

        if (pausableScenes.Contains(SceneController.GetActualSceneIndex()) && (pauseMenuCanvas != null && pauseMenuCanvasGroup != null)) // comprovar si es pot pausar l'escena
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

    private void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(pauseMenuCanvasGroup, pauseMenuCanvasGroup.alpha, 1f, 0.5f));
    }
    
    private void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(pauseMenuCanvasGroup, pauseMenuCanvasGroup.alpha, 0f, 0.5f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime/duration);
            yield return null;
        }
        cg.alpha = end;

        if (end == 0f)
        {
            pauseMenuCanvas.SetActive(isGamePaused);
        }
    }
}
