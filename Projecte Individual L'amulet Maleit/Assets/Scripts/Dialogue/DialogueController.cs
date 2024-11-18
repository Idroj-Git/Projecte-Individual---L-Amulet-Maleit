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
    [SerializeField] TMP_Text nameText;
    private float lettersPerSecond = 40;

    private bool dBoxOpen = false;

    [SerializeField] PlayerMovement player;
    [SerializeField] PlayerSettings playerSettings;
    private bool nextButtonPressed = false, canSkipDialogue = false;

    private TextStorage actualDialogue;
    private int currentLine = 0;

    private Coroutine disableInitialClickCoroutine;
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

    private void OnEnable()
    {
        InputController.OnInteractDialogue += SetNextButtonPressed;
    }
    private void OnDisable()
    {
        InputController.OnInteractDialogue -= SetNextButtonPressed;
    }

    private void SetNextButtonPressed() // Controla el input del click
    {
        if (dBoxOpen)
        {
            if (!nextButtonPressed)
            {
                Debug.Log("Click!");
                nextButtonPressed = true;
                StartCoroutine(DisableInitialClick());
                StartCoroutine(InteractCooldown());
            }
            //else
            //{
            //    Debug.Log("NextButtonPressed"); <-- No s'executa mai
            //}
        }
        //StartCoroutine(NextButtonPressedWait());
    }

    private IEnumerator InteractCooldown() // Retorna el valor de nextButtonPressed a false.
    {
        yield return new WaitForSeconds(0.1f);
        nextButtonPressed = !nextButtonPressed;
    }

    //private IEnumerator NextButtonPressedWait()
    //{
    //    yield return new WaitForEndOfFrame();
    //    StartCoroutine(InteractCooldown());
    //    if (dBoxOpen)
    //    {
    //        if (!nextButtonPressed)
    //        {
    //            Debug.Log("Click!");
    //            StartCoroutine(DisableInitialClick());
    //            nextButtonPressed = true;
    //            StartCoroutine(InteractCooldown());
    //        }
    //        else
    //        {
    //            Debug.Log("NextButtonPressed");
    //        }
    //    }
    //}

    private void Start()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
    }

    private void SetTextName(string name)
    {
        nameText.text = name;
    }

    public void ShowDialogue(TextStorage dialogue) // METODE PRINCIPAL
    {
        Debug.Log("truers");
        dBoxOpen = true;
        textBox.SetActive(dBoxOpen);
        player.SetCanMove(!dBoxOpen);
        actualDialogue = dialogue;
        nextButtonPressed = false;
        if (disableInitialClickCoroutine != null)
        {
            StopCoroutine(disableInitialClickCoroutine);
            disableInitialClickCoroutine = null;
        }
        disableInitialClickCoroutine = StartCoroutine(DisableInitialClick());

        string line = actualDialogue.Lines[currentLine];
        int separatorIndex = line.IndexOf('>');

        if (separatorIndex != -1) // Asegura que hi ha nom,
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

    public void CloseDialogueBox()
    {
        dBoxOpen = false;
        textBox.SetActive(dBoxOpen);
        player.SetCanMove(!dBoxOpen);
        playerSettings.SetCanInteract(true);
    }

    public IEnumerator TypeDialogue(string line) // OJO QUE EL TEXT NO TINGUI MÉS DE X CHARS!!!
    {
        yield return new WaitForEndOfFrame();
        nextButtonPressed = false;
        dialogueText.text = string.Empty; // lo mateix que posar ->    = "";
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

        if (currentLine < actualDialogue.Lines.Count - 1)
        {
            yield return StartCoroutine(WaitForInteract(() => nextButtonPressed));
            nextButtonPressed = false;
            currentLine++;
            ShowDialogue(actualDialogue);
        }
        else
        {
            yield return StartCoroutine(WaitForInteract(() => nextButtonPressed));
            nextButtonPressed = false;
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

    private IEnumerator DisableInitialClick() // Evita que el botó es pugui premer fins que passa cert temps.
    {
        canSkipDialogue = false;

        yield return new WaitForSeconds(1.0f);

        canSkipDialogue = true;
    }
}
