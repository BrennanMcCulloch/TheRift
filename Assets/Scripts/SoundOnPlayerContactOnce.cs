using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnPlayerContactOnce : MonoBehaviour
{
    // Play the sudio source's sound when the player touches this, then disable the collider
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<SphereCollider>().enabled = false;
        }
    }
    
}
