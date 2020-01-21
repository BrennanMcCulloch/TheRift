using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narration : MonoBehaviour
{
    public static Narration instance;
    private static AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        source = instance.gameObject.GetComponent<AudioSource>();
    }

    // Plays an audio clip
    public static void Narrate (AudioClip newClip)
    {
        source.clip = newClip;
        source.Play();
    }
}
