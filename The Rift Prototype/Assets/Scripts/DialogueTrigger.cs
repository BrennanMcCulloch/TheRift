using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        //Set state to talking
        StateManager.setState((int)StateManager.StateEnum.Talking);
        //Begin dialogue
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}