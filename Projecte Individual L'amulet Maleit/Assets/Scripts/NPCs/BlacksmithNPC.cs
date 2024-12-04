using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithNPC : NPCController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interacted()
    {
        StartCoroutine(DialogueAndShopRoutine());
    }

    IEnumerator DialogueAndShopRoutine()
    {
        yield return null;
        StartDialogue();
        yield return new WaitUntil(() => DialogueController.Instance.GetHasDialogueFinished());
    }
}
