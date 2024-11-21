using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoryController
{
    private static DialogueContainer dialogueContainer;

    public static void LoadDialoguesFromJson(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile != null)
        {
            dialogueContainer = JsonUtility.FromJson<DialogueContainer>(jsonFile.text);
            Debug.Log("Diálogos cargados exitosamente.");
        }
        else
        {
            Debug.LogError($"No se encontró el archivo JSON: {fileName}");
        }
    }

    public static Dialogue GetDialogueById(int id)
    {
        foreach (var dialogue in dialogueContainer.dialogues)
        {
            if (dialogue.id == id)
                return dialogue;
        }
        return null;
    }
}
