using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    private bool hasDialogueFinished = true;

    public bool GetHasDialogueFinished()
    {
        return this.hasDialogueFinished;
    }
    public void SetHasDialogueFinished(bool hasDialogueFinished)
    {
        this.hasDialogueFinished = hasDialogueFinished;
    }

    [SerializeField] GameObject textBox;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text nameText;
    private float lettersPerSecond = 40;

    private bool dBoxOpen = false;

    [SerializeField] PlayerMovement player;
    [SerializeField] PlayerSettings playerSettings;
    private bool nextButtonPressed = false;


    float timeSinceDialogueBoxOpened = 0;
    private bool canSkipDialogue
    {
        get { return timeSinceDialogueBoxOpened >= 0.3f; }
    }

    private TextStorage actualDialogue;
    private Dialogue actualStoryDialogue;
    private int actualDialogueID;
    private int currentLine = 0;

    float maxInteractionCooldown = 0.03f;
    float interactionCooldown = 0.03f;
    bool hasInteractionCooldownFinished
    {
        get { return interactionCooldown == 0; }
    }
    //private int maxLine;

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
        if (dBoxOpen)
        {
            CloseDialogueBox();
        }
    }

    private void Update()
    {
        interactionCooldown = Mathf.Max(interactionCooldown - Time.deltaTime, 0); // Comprova si interactionCooldown es més gran que 0, si ho és: es resta el deltaTime a interactCD. Sino es queda a 0.
        timeSinceDialogueBoxOpened = dBoxOpen ? timeSinceDialogueBoxOpened + Time.deltaTime : 0; // el ? es el mateix que posar un if dBoxOpen { sumadeltatime } else { 0 }
    }


    private void SetNextButtonPressed() // Controla el input del click
    {
        if (dBoxOpen)
        {
            if (!nextButtonPressed)
            {
                //Debug.Log("Click!");
                nextButtonPressed = true;
            }
            //else
            //{
            //    Debug.Log("NextButtonPressed"); <-- No s'executa mai
            //}
        }
    }
    private void SetTextName(string name)
    {
        nameText.text = name;
    }

    public void CloseDialogueBox()
    {
        SetDialogueBoxStatus(false);
        hasDialogueFinished = true;
        playerSettings = FindAnyObjectByType<PlayerSettings>();
        if (playerSettings != null)
            playerSettings.SetCanInteract(true); // es posa a false en el metode de interactuar de totes les diferents interaccions
        InputController.OnInteractDialogue -= SetNextButtonPressed;
        //Debug.Log("Close");
    }

    public void OpenDialogueBox()
    {
        hasDialogueFinished = false;
        SetDialogueBoxStatus(true);
        InputController.OnInteractDialogue += SetNextButtonPressed;
        //Debug.Log("Open");
    }

    void SetDialogueBoxStatus(bool open)
    {
        dBoxOpen = open;
        textBox.SetActive(open);
        player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
            player.SetCanMove(!open);
    }

    public void ShowDialogue(TextStorage dialogue) // METODE PRINCIPAL
    {
        //Debug.Log("ShowDialogue");
        hasDialogueFinished = false;
        if (!dBoxOpen)
        {
            OpenDialogueBox();
        }
        actualDialogue = dialogue;
        nextButtonPressed = false;
        timeSinceDialogueBoxOpened = 0;
        //Debug.Log("nextButtonPressedIsFalse");

        string line = actualDialogue.Lines[currentLine];
        int separatorIndex = line.IndexOf('>');

        if (separatorIndex != -1) // Asegura que hi ha nom
        {
            string characterName = line.Substring(0, separatorIndex + 1).Trim();
            string dialogueText = line.Substring(separatorIndex + 1).Trim();

            SetTextName(characterName);
            StartCoroutine(TypeDialogue(dialogueText));
        }
        else
        {
            SetTextName("<???>"); // Si no troba el nom es posa ??? per default.
            StartCoroutine(TypeDialogue(line));
        }
    }

    public void ShowStoryDialogue(int dialogueId) // METODE secundari
    {
        //Debug.Log("ShowDialogue");
        if (!dBoxOpen)
        {
            OpenDialogueBox();
        }
        actualDialogueID = dialogueId;
        actualStoryDialogue = StoryController.GetDialogueById(dialogueId);
        if (actualStoryDialogue == null)
        {
            Debug.LogError($"No se pudo cargar el diálogo con ID: {dialogueId}");
            CloseDialogueBox();
            return;
        }
        nextButtonPressed = false;
        timeSinceDialogueBoxOpened = 0;
        //Debug.Log("nextButtonPressedIsFalse");

        string line = actualStoryDialogue.lines[currentLine].text;
        string characterName = actualStoryDialogue.lines[currentLine].name;

        if (characterName != "") // Asegura que hi ha nom
        {
            SetTextName(characterName);
            StartCoroutine(TypeDialogue(line));
        }
        else
        {
            SetTextName("<???>"); // Si no troba el nom es posa ??? per default.
            StartCoroutine(TypeDialogue(line));
        }
    }

    public IEnumerator TypeDialogue(string line) // OJO QUE EL TEXT NO TINGUI MÉS DE X CHARS!!!
    {
        nextButtonPressed = false;
        yield return null;
        dialogueText.text = string.Empty; // lo mateix que posar ->    = "";
        interactionCooldown = maxInteractionCooldown;
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            if (nextButtonPressed && canSkipDialogue)
            {
                dialogueText.text = line;
                nextButtonPressed = false;
                //canSkipDialogue = false;
                break; // Es l'única manera que se'm acudeix per poder sortir del foreach abans de que acabi...
            }
            else
            {
                yield return new WaitForSeconds(1f / lettersPerSecond);
            }
        }
        //yield return new WaitForSeconds(3f); // dialogue interact

        if (actualDialogueID != 0) // comprova si es un text de historia o no
        {
            if (currentLine < actualStoryDialogue.lines.Length - 1)
            {
                yield return StartCoroutine(WaitForInteract(() => nextButtonPressed && canSkipDialogue));
                nextButtonPressed = false;
                currentLine++;
                ShowStoryDialogue(actualDialogueID);
            }
            else
            {
                yield return StartCoroutine(WaitForInteract(() => nextButtonPressed && canSkipDialogue));
                nextButtonPressed = false;
                currentLine = 0;
                actualDialogue = null;
                actualStoryDialogue = null;
                actualDialogueID = 0;
                if (dBoxOpen)
                {
                    CloseDialogueBox();
                }
            }
        }
        else
        {
            if (currentLine < actualDialogue.Lines.Count - 1)
            {
                yield return StartCoroutine(WaitForInteract(() => nextButtonPressed && canSkipDialogue));
                nextButtonPressed = false;
                currentLine++;
                ShowDialogue(actualDialogue);
            }
            else
            {
                yield return StartCoroutine(WaitForInteract(() => nextButtonPressed && canSkipDialogue));
                nextButtonPressed = false;
                currentLine = 0;
                actualDialogue = null;
                actualStoryDialogue = null;
                actualDialogueID = 0;
                if (dBoxOpen)
                {
                    CloseDialogueBox();
                }
            }
        }
    }

    private IEnumerator WaitForInteract(Func<bool> condition, float checkInterval = 0.1f)
    {
        while (!condition())
        {
            yield return new WaitForSeconds(checkInterval);
        }
    }

    internal void StartNPCDialogue(TextStorage text)
    {
        hasDialogueFinished = false;
        ShowDialogue(text);
    }


}
