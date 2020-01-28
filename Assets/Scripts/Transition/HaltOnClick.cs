using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Disable Jitter and WordScramble of children on click
    private void OnMouseUpAsButton()
    {
        Jitter[] childJitters = GetComponentsInChildren<Jitter>();
        WordScramble[] childWordScrambles = GetComponentsInChildren<WordScramble>();
        for(int i = 0; i < childJitters.Length; i++)
        {
            childJitters[i].enabled = false;
            childWordScrambles[i].enabled = false;
        }
    }
}
