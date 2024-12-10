using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveStoryEvents : MonoBehaviour
{
    [SerializeField] GameObject storyObject6;
    [SerializeField] GameObject cursedAmulet;
    [SerializeField] Animator draculaAnimator;
    [SerializeField] GameObject draculaObject;

    // Start is called before the first frame update
    void Start()
    {
        if (RuntimeGameSettings.Instance.GetStoryProgression() == 7)
        {
            ToggleMoveInteractPause(false);
            StartCoroutine(DraculaDefeated()); // Escena 2
        }
        else if (RuntimeGameSettings.Instance.GetStoryProgression() == 6)
        {
            storyObject6.SetActive(true);
            draculaObject.SetActive(true);
            cursedAmulet.SetActive(false);
        }
        else
        {
            storyObject6.SetActive(false);
            draculaObject.SetActive(false);
            cursedAmulet.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleChildCollision(Collider2D collision, GameObject gameObject)
    {
        if (collision.CompareTag("Player"))
        {
            switch (RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/)
            {
                case 6:
                    storyObject6.SetActive(false);
                    StartCoroutine(DraculaStartFight()); // Escena 1
                    break;
            }
        }
    }

    private IEnumerator DraculaStartFight()
    {
        CallStoryDialogue();
        yield return new WaitUntil(() => DialogueController.Instance.GetHasDialogueFinished());
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        RuntimeGameSettings.Instance.SetPlayerLastPosition(playerMovement.GetPosition());
        SceneController.LoadBattleScene(); 
    }

    private IEnumerator DraculaDefeated()
    {
        CallStoryDialogue();
        yield return new WaitUntil(()=>DialogueController.Instance.GetHasDialogueFinished());
        ToggleMoveInteractPause(false);
        draculaAnimator.SetTrigger("disappearing");
        yield return new WaitForSeconds(0.5f);
        CallStoryDialogue();
        ToggleMoveInteractPause(false);
        yield return new WaitForSeconds(1f);
        draculaObject.SetActive(false);
        cursedAmulet.SetActive(true);
        yield return new WaitUntil(() => DialogueController.Instance.GetHasDialogueFinished());
        ToggleMoveInteractPause(true);
    }

    private void CallStoryDialogue()
    {
        Debug.Log(RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/);
        DialogueController.Instance.ShowStoryDialogue(RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/);
        RuntimeGameSettings.Instance.IncreaseStoryProgression();
        //PlayerPrefs.SetInt(RuntimeGameSettings.storyFlagName, RuntimeGameSettings.Instance.GetStoryProgression() + 1);
        PlayerPrefs.Save();
        Debug.Log(RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/);
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
