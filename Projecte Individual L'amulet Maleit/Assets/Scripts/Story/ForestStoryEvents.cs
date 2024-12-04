using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestStoryEvents : MonoBehaviour
{
    [SerializeField] GameObject storyObject2;
    [SerializeField] GameObject storyObject3;
    //[SerializeField] GameObject missingTreeObject;
    [SerializeField] GameObject enemyGrassObject;

    public static int obtainedTrees = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/ == 2)
        {
            storyObject2.SetActive(true);
            storyObject3.SetActive(false);
            //missingTreeObject.SetActive(true);
            enemyGrassObject.SetActive(false);
        }
        else if (RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/ == 3)
        {
            storyObject2.SetActive(false);
            storyObject3.SetActive(true);
            //missingTreeObject.SetActive(false);
            enemyGrassObject.SetActive(true);
        }
        else
        {
            storyObject2.SetActive(false);
            storyObject3.SetActive(false);
            //missingTreeObject.SetActive(false);
            enemyGrassObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (obtainedTrees >= 2 && RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/ == 3)
        {
            EnoughTreesCollected();
        }
    }

    public void HandleChildCollision(Collider2D collision, GameObject gameObject)
    {
        if (collision.CompareTag("Player"))
        {
            switch (RuntimeGameSettings.Instance.GetStoryProgression()/*PlayerPrefs.GetInt(RuntimeGameSettings.storyFlagName)*/)
            {
                case 2:
                    storyObject2.SetActive(false);
                    CallStoryDialogue();
                    //missingTreeObject.SetActive(false);
                    //enemyGrassObject.SetActive(true);
                    //storyObject3.SetActive(true);
                    break;
                case 3:
                    storyObject3.SetActive(false);
                    CallStoryDialogue();
                    break;
            }
        }
    }

    private void EnoughTreesCollected()
    {
        //storyObject2.SetActive(false);
        //CallStoryDialogue();
        enemyGrassObject.SetActive(true);
        storyObject3.SetActive(true);
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
}
