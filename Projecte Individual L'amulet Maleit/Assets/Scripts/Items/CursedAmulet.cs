using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedAmulet : ItemController
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] GameObject loadingText;

    public override void Interacted() // Per fer override ÉS NECESSARI QUE SIGUI VIRTUAL!
    {
        playerSettings.SetCanInteract(false);
        //RuntimeGameSettings.Instance.SetStoryProgression(9);
        //PlayerPrefs.SetInt(RuntimeGameSettings.storyFlagName, 20); // Implementació pendent.
        StartCoroutine(EndingScene());
    }

    private void CallStoryDialogue()
    {
        Debug.Log(RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/);
        DialogueController.Instance.ShowStoryDialogue(RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/);
        RuntimeGameSettings.Instance.IncreaseStoryProgression();
        //PlayerPrefs.SetInt(RuntimeGameSettings.storyFlagName, PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) + 1);
        PlayerPrefs.Save();
        Debug.Log(RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/);
    }

    private IEnumerator EndingScene()
    {
        loadingText.SetActive( false );
        CallStoryDialogue();
        yield return new WaitForEndOfFrame();
        yield return WaitForDialogueFinished(() => DialogueController.Instance.GetHasDialogueFinished());
        ToggleMoveInteractPause(false);
        //playerSettings.SetCanInteract(false);
        CanvasFade.Instance.FadeCanvas();
        Time.timeScale = 1f; // el fadein ho posa a 0. Per que surti el text amb el fons en negre necessito que sigui 1.
        RuntimeGameSettings.Instance.SetStoryProgression(21);
        CallStoryDialogue();
        DialogueController.Instance.SetHasDialogueFinished(false);
        yield return WaitForDialogueFinished(() => DialogueController.Instance.GetHasDialogueFinished());
        //playerSettings.SetCanInteract(false);
        SceneController.LoadWinScene();
    }

    private IEnumerator WaitForDialogueFinished(Func<bool> condition, float checkInterval = 0.6f)
    {
        while (!condition())
        {
            yield return new WaitForSeconds(checkInterval);
        }
        playerSettings.SetCanInteract(false); // Necessari ja que al tancar un dialeg es posa a true.
    }

    private void ToggleMoveInteractPause(bool can)
    {
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.SetCanMove(can);
        ButtonController buttonController = FindAnyObjectByType<ButtonController>();
        if (buttonController != null)
            buttonController.SetCanPauseGame(can);
        PlayerSettings playerSettings = FindAnyObjectByType<PlayerSettings>();
        if (playerSettings != null)
            playerSettings.SetCanInteract(can);
    }
}
