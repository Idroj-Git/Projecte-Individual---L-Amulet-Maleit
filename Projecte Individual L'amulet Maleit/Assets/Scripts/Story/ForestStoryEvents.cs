using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestStoryEvents : MonoBehaviour
{
    [SerializeField] GameObject storyObject2;
    [SerializeField] GameObject storyObject3;
    [SerializeField] GameObject missingTreeObject;
    [SerializeField] GameObject enemyGrassObject;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) == 2)
        {
            storyObject2.SetActive(true);
            storyObject3.SetActive(false);
            missingTreeObject.SetActive(true);
            enemyGrassObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName) == 3)
        {
            storyObject2.SetActive(false);
            storyObject3.SetActive(true);
            missingTreeObject.SetActive(false);
            enemyGrassObject.SetActive(true);
        }
        else
        {
            storyObject2.SetActive(false);
            storyObject3.SetActive(false);
            missingTreeObject.SetActive(false);
            enemyGrassObject.SetActive(true);
        }
    }

    public void HandleChildCollision(Collider2D collision, GameObject gameObject)
    {
        if (collision.CompareTag("Player"))
        {
            switch (PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName))
            {
                case 2:
                    storyObject2.SetActive(false);
                    CallStoryDialogue();
                    missingTreeObject.SetActive(false);
                    enemyGrassObject.SetActive(true);
                    storyObject3.SetActive(true);
                    break;
                case 3:
                    storyObject3.SetActive(false);
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
