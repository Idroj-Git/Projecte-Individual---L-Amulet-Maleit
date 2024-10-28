using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }


    [SerializeField] GameObject textBox;
    [SerializeField] TMP_Text dialogueText;
    private float lettersPerSecond = 40;

    private bool dBoxOpen = false;

    [SerializeField] PlayerMovement player;

    private void Awake() // Singleton
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
    }

    public void ShowDialogue(TextStorage text)
    {
        dBoxOpen = true;
        textBox.SetActive(dBoxOpen);
        player.SetCanMove(!dBoxOpen);
        StartCoroutine(TypeDialogue(text.Lines[0]));
    }

    public void CloseDialogueBox()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
        player.SetCanMove(!dBoxOpen);
    }

    public IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = string.Empty; // lo mateix que posar ->    = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(3f); // dialogue interact
        CloseDialogueBox();
    }
}
