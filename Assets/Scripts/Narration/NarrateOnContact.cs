using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrateOnContact : MonoBehaviour
{
    public AudioClip narrationClip;

    // Play the sudio source's sound when the player touches this, then disable the collider
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Narration.Narrate(narrationClip);
        }
    }
    
}
