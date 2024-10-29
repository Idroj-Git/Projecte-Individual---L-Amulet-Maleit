using System;
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
    [SerializeField] PlayerSettings playerSettings;
    private bool nextButtonPressed = false;

    private TextStorage actualDialogue;
    private int currentLine = 0;
//    private int maxLine;

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

    private void OnEnable()
    {
        InputController.OnInteractDialogue += SetNextButtonPressed;
    }
    private void OnDisable()
    {
        InputController.OnInteractDialogue -= SetNextButtonPressed;
    }

    private void SetNextButtonPressed()
    {
        if (!nextButtonPressed)
        {
            nextButtonPressed = true;
            StartCoroutine(InteractCooldown());
        }
    }

    private IEnumerator InteractCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        nextButtonPressed = false;
    }

    private void Start()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
    }

    public void ShowDialogue(TextStorage dialogue)
    {
        dBoxOpen = true;
        textBox.SetActive(dBoxOpen);
        player.SetCanMove(!dBoxOpen);
        actualDialogue = dialogue;
        StartCoroutine(TypeDialogue(actualDialogue.Lines[currentLine]));
    }

    public void CloseDialogueBox()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
        player.SetCanMove(!dBoxOpen);
        playerSettings.SetCanInteract(true);
    }

    public IEnumerator TypeDialogue(string line) // OJO QUE EL TEXT NO TINGUI MÉS DE X CHARS!!!
    {
        dialogueText.text = string.Empty; // lo mateix que posar ->    = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        //yield return new WaitForSeconds(3f); // dialogue interact
        if (currentLine < actualDialogue.Lines.Count - 1) 
        {
            yield return StartCoroutine(WaitForInteract(() => nextButtonPressed));
            currentLine++; 
            ShowDialogue(actualDialogue);
        }
        else
        {
            yield return StartCoroutine(WaitForInteract(() => nextButtonPressed));
            currentLine = 0;
            actualDialogue = null;
            CloseDialogueBox();
        }
    }

    private IEnumerator WaitForInteract(Func<bool> condition, float checkInterval = 0.1f)
    {
        while (!condition())
        {
            yield return new WaitForSeconds(checkInterval);
        }
    }

}
