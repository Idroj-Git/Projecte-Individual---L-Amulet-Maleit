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
