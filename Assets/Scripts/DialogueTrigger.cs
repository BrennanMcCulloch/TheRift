using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DialogueTree;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private Material material;

    private bool interactable;

    void Start() {
        material = gameObject.GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Todo (matt) - These methods assume that the player is the only object that will be moving around and triggering things.
        // if at some point this is no longer the case, add logic to bail unless the colliding object is the player.
        material.color = Color.green;
        interactable = true;
    }

    private void OnTriggerExit(Collider other) {
        material.color = Color.yellow;
        interactable = false;
    }

    private void OnMouseUpAsButton()
    {
        if (interactable && dialogue != null)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
