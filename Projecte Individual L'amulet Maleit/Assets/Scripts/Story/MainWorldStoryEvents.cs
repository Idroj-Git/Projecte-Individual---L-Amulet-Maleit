using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldStoryEvents : MonoBehaviour
{
    [SerializeField] GameObject storyObject1;
    [SerializeField] GameObject storyObject4;
    [SerializeField] GameObject storyObject5;

    [SerializeField] GameObject pathBlocking;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) == 1)
        {
            storyObject1.SetActive(true);
            storyObject4.SetActive(false);
            storyObject5.SetActive(false);
            pathBlocking.SetActive(true);
        }
        else if (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) == 4)
        {
            storyObject1.SetActive(false);
            storyObject4.SetActive(true);
            storyObject5.SetActive(false);
            pathBlocking.SetActive(true);
        }
        else if (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) == 5)
        {
            storyObject1.SetActive(false);
            storyObject4.SetActive(false);
            storyObject5.SetActive(true);
            pathBlocking.SetActive(true);
        }
        else
        {
            storyObject1.SetActive(false);
            storyObject4.SetActive(false);
            storyObject5.SetActive(false);
            pathBlocking.SetActive(false);
        }
    }

    public void HandleChildCollision(Collider2D collision, GameObject gameObject) // Quan un story object activa un altra es desactiva a ell mateix.
    {
        if (collision.CompareTag("Player"))
        {
            switch (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName))
            {
                case 1:
                    storyObject1.SetActive(false);
                    CallStoryDialogue();
                    break;
                case 4:
                    storyObject4.SetActive(false);
                    storyObject5.SetActive(true);
                    CallStoryDialogue();
                    break;
                case 5:
                    storyObject5.SetActive(false);
                    pathBlocking.SetActive(false);
                    CallStoryDialogue();
                    break;
            }
        }
    }

    private void CallStoryDialogue()
    {
        Debug.Log(PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName));
        DialogueController.Instance.ShowStoryDialogue(PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName));
        PlayerPrefs.SetInt(RuntimeGameSettings.storyFlagName, PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) + 1);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName));
    }
}
