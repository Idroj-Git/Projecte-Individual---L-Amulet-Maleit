using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] TextStorage text; // FER LLISTA DE LA CLASSE TextStorage, així puc definir dins la classe qui parla amb un string nom;!!
    [SerializeField] Rigidbody2D rb;
    public PlayerSettings playerSettings;


    protected void StartDialogue()
    {
        DialogueController.Instance.StartNPCDialogue(text);
    }

    public virtual void Interacted()
    {
        playerSettings.SetCanInteract(false);
        StartDialogue();
    }
}
