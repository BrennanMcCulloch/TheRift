using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrateOnContact : MonoBehaviour
{
    public AudioClip narrationClip;

    private bool narrationReady = false;

    // Initialize narrationReady Properly
    void Start ()
    {
        if (narrationClip != null) narrationReady = true;
    }

    // Play the sudio source's sound when the player touches this, then disable the collider
    void OnTriggerEnter(Collider other)
    {
        if (narrationReady == true && other.gameObject.tag == "Player")
        {
            Narration.Narrate(narrationClip);
            narrationReady = false;
        }
    }
    
}
