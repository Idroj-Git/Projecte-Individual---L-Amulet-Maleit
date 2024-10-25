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
    private float lettersPerSecond = 10;

    private bool dBoxOpen = false;

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


    public void ShowDialogue(TextStorage text)
    {
        dBoxOpen = true;
        textBox.SetActive(dBoxOpen);
        StartCoroutine(TypeDialogue(text.Lines[0]));
    }

    public void CloseDialogueBox()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
    }

    public IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(3f);
        CloseDialogueBox();
    }
}
