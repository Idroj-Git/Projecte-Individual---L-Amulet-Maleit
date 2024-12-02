using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] PlayerSettings playerSettings;
    //[SerializeField] ItemType type;

    public virtual void Interacted()
    {
        //playerSettings.SetCanInteract(false);
        //Debug.Log("Type: " + type);
        //switch (type)
        //{
        //    case ItemType.CursedAmulet:
        //        PlayerPrefs.SetInt(RuntimeGameSettings.storyFlagName, 20); // Implementació pendent.
        //        StartCoroutine(EndingScene());
        //        break;
        //}
    }

    //public enum ItemType
    //{
    //    CursedAmulet,
    //    Healing
    //}
    //private void CallStoryDialogue()
    //{
    //    Debug.Log(PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName));
    //    DialogueController.Instance.ShowStoryDialogue(PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName));
    //    PlayerPrefs.SetInt(RuntimeGameSettings.storyFlagName, PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) + 1);
    //    PlayerPrefs.Save();
    //    Debug.Log(PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName));
    //}

    //private IEnumerator EndingScene()
    //{
    //    CallStoryDialogue();
    //    yield return new WaitForEndOfFrame();
    //    yield return WaitForDialogueFinished(() => DialogueController.Instance.hasDialogueFinished);
    //    //playerSettings.SetCanInteract(false);
    //    CanvasFade.Instance.FadeCanvas();
    //    Time.timeScale = 1f; // el fadein ho posa a 0. Per que surti el text amb el fons en negre necessito que sigui 1.
    //    CallStoryDialogue();
    //    DialogueController.Instance.hasDialogueFinished = false;
    //    yield return WaitForDialogueFinished(() => DialogueController.Instance.hasDialogueFinished);
    //    //playerSettings.SetCanInteract(false);
    //    SceneController.LoadWinScene();
    //}

    //private IEnumerator WaitForDialogueFinished(Func<bool> condition, float checkInterval = 0.6f)
    //{
    //    while (!condition())
    //    {
    //        yield return new WaitForSeconds(checkInterval);
    //    }
    //    playerSettings.SetCanInteract(false); // Necessari ja que al tancar un dialeg es posa a true.
    //}
}
