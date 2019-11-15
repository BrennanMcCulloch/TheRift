using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    public string name;

    public UnityEvent thingToDoOnEntry;

    public Event neutral = null;

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

