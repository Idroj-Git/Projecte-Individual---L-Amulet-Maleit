using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] TextStorage text;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerSettings playerSettings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //StartDialogue();
    }

    private void StartDialogue() // hacer public? (no funciona creo)
    {
        DialogueController.Instance.ShowDialogue(text);
    }

    public void Interacted()
    {
        playerSettings.SetCanInteract(false);
        StartDialogue();
    }
}
