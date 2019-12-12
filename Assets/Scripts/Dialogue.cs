using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    new public string name;

    public Conditional neutral;

    public UnityEvent thingToDoOnEntry;

    public List<Talkeys> sentences;
}

[System.Serializable]
public class Conditional : SerializableCallback<bool>
{

}

///    public SerializableCallback<bool> neutral = null;
/// 
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

