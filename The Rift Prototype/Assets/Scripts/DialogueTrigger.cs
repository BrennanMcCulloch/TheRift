using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        //Attempt to begin dialogue
        if(FindObjectOfType<DialogueManager>().StartDialogue(dialogue))
        {
            //Set state to talking if dialogue properly started
            StateManager.setState((int)StateManager.StateEnum.Talking);
        }
    }
}