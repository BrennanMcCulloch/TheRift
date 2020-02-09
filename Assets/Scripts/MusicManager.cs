using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//BAD CODE TO BE REWRITTEN
public class MusicManager : Singleton<MusicManager>
{
    public AudioSource source1;
    public AudioSource source2;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMusic()
    {
        source1.enabled = !source1.enabled;
        source2.enabled = !source2.enabled;
    }
}
