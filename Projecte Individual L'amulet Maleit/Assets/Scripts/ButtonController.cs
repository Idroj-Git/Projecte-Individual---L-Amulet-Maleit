using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private bool isGamePaused;
    private int[] pausableScenes, unsavableScenes;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] Button continueButton;
    private void Start()
    {
        pausableScenes = new int[] { SceneController.GetMainWorldIndex(), SceneController.GetForestIndex(), SceneController.GetBattleIndex(), SceneController.GetCaveIndex()};
        unsavableScenes = new int[] { SceneController.GetMainMenuIndex(), SceneController.GetBattleIndex(), SceneController.GetWinIndex(), SceneController.GetLoseIndex()};


        isGamePaused = false;

        if (pausableScenes.Contains(SceneController.GetActualSceneIndex()) && pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(isGamePaused);
        }

        if (continueButton != null)
        {
            if (PlayerPrefs.GetInt("GameStarted")  == 0) // Serveix per saber si hi ha una partida començada, una vegada creada una partida sempre serà else a no ser que es borri de la carpeta
            {
                continueButton.interactable = false; // Reset de la historia obligatori
            }
            else
                continueButton.interactable = true;
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
        //RuntimeGameSettings.Instance.SetLastScene(1); // Reset de tot.
        //RuntimeGameSettings.Instance.SetPlayerLastPosition(new Vector2(16, 2));
        if (!unsavableScenes.Contains(SceneController.GetActualSceneIndex()))
            RuntimeGameSettings.Instance.SaveGame();
        SceneController.LoadMainMenuScene();
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("GameStarted", 1);
        RuntimeGameSettings.Instance.LoadGame(); // Carrega les stats anteriors
        SceneController.LoadSceneByIndex(RuntimeGameSettings.Instance.GetLastScene());
    }

    public void StartNewGame() // mainWorld
    {
        PlayerPrefs.SetInt("GameStarted", 1);
        //PlayerPrefs.SetInt("StoryFlag", 1); // Reset de la història // Es fa dins de SetBaseStats
        RuntimeGameSettings.Instance.SetBaseStats(); // Carrega les stats default
        SceneController.LoadMainWorldScene();
    }

    public void ExitGame() //apagar el joc
    {
        //PlayerPrefs.SetInt("GameStarted", 0);
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
        PlayerSettings playerSettings = FindAnyObjectByType<PlayerSettings>();
        if (playerSettings != null)
            playerSettings.SetCanInteract(false);
        pauseMenuCanvasGroup.alpha = 0f; // Posar en el inspector (prefab) el canvasGroup.alpha = 0, quan ho tingui acabat
        
        FadeIn();
        Time.timeScale = 0.0f;
    }

    private void ClosePauseMenu()
    {
        isGamePaused = false;
        FadeOut();

        PlayerSettings playerSettings = FindAnyObjectByType<PlayerSettings>();
        if (playerSettings != null)
            playerSettings.SetCanInteract(true);
        Time.timeScale = 1.0f;
    }

    public void PauseMenu()
    {

        if (pausableScenes.Contains(SceneController.GetActualSceneIndex()) 
            && pauseMenuCanvas != null 
            && pauseMenuCanvasGroup != null
            && DialogueController.Instance.GetHasDialogueFinished()) // comprovar si es pot pausar l'escena
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
