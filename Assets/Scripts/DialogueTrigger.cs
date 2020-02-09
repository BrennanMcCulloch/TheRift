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
    public AudioClip nearbyNarrationClip;

    private Material material;
    private Color originalColor;
    private bool interactable;
    private bool nearbyNarrationReady = false;

    void Start() {
        material = gameObject.GetComponent<MeshRenderer>().material;
        originalColor = material.color;
        if (nearbyNarrationClip != null) nearbyNarrationReady = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //only activate when the player is near
        if (other.gameObject.tag == "Player")
        {
            material.color = Color.green;
            interactable = true;
            //play narration if possible
            if (nearbyNarrationReady == true)
            {
                Narration.Narrate(nearbyNarrationClip);
                nearbyNarrationReady = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //only deactivate when the player leaves
        if (other.gameObject.tag == "Player")
        {
            material.color = originalColor;
            interactable = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (MasterDialogueTrigger.instance.enabled && interactable && dialogue != null)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
