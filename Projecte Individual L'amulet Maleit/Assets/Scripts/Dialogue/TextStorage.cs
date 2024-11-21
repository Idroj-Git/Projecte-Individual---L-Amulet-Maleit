using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextStorage
{
    //[SerializeField] string name;
    [SerializeField] List<string> lines;

    public List<string> Lines
    {
        get { return lines; }
    }
    
    //public string GetName() { return name; }
}


[System.Serializable]
public class DialogueContainer
{
    public Dialogue[] dialogues;
}

[System.Serializable]
public class Dialogue
{
    public int id;
    public Line[] lines;
}

[System.Serializable]
public class Line
{
    public string name;
    public string text;
}

