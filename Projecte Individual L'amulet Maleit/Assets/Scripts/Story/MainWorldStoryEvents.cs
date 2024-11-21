using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldStoryEvents : MonoBehaviour
{
    [SerializeField] GameObject storyObject1;
    [SerializeField] GameObject storyObject4;
    [SerializeField] GameObject storyObject5;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("StoryFlag") == 1)
        {
            storyObject1.SetActive(true);
            storyObject4.SetActive(false);
            storyObject5.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("StoryFlag") == 4)
        {
            storyObject1.SetActive(false);
            storyObject4.SetActive(true);
            storyObject5.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("StoryFlag") == 5)
        {
            storyObject1.SetActive(false);
            storyObject4.SetActive(false);
            storyObject5.SetActive(true);
        }
        else
        {
            storyObject1.SetActive(false);
            storyObject4.SetActive(false);
            storyObject5.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandleChildCollision(Collider2D collision, GameObject gameObject)
    {
        switch (PlayerPrefs.GetInt("StoryFlag"))
        {
            case 1:
                storyObject1.SetActive(false);
                CallStoryDialogue();
                break;
            case 4:
                storyObject4.SetActive(false);
                CallStoryDialogue();
                break;
            case 5:
                storyObject1.SetActive(false);
                CallStoryDialogue();
                break;
        }
    }

    private void CallStoryDialogue()
    {
        DialogueController.Instance.ShowStoryDialogue(PlayerPrefs.GetInt("StoryFlag"));
        PlayerPrefs.SetInt("StoryFlag", PlayerPrefs.GetInt("StoryFlag") + 3);
    }
}
