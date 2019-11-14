using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    public List<Talkeys> sentences;
}

[System.Serializable]
public class Talkeys
{
    [TextArea(3, 10)] //min, max
    public string whatToSay;
    public bool isChoice;
    public int Body;
    public int Mind;
    public int Soul;
    public Dialogue nextDialogueSuccess;
    public Dialogue nextDialogueFail;
}