using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltOnClick : MonoBehaviour
{
    public float timeBetweenStops = 0.2f;

    // Disable Jitter and WordScramble of children on click
    private void OnMouseUpAsButton()
    {
        Debug.Log("click");//test
        StartCoroutine(HaltInSequence());
    }

    // Turns off Jitter and Scramble in a set sequence
    IEnumerator HaltInSequence()
    {
        Debug.Log("ran");//test
        Jitter[] childJitters = GetComponentsInChildren<Jitter>();
        WordScramble[] childWordScrambles = GetComponentsInChildren<WordScramble>();
        for(int i = 0; i < childJitters.Length; i++)
        {
            childWordScrambles[i].enabled = false;
            yield return new WaitForSeconds(timeBetweenStops);
        }
        for(int i = 0; i < childJitters.Length; i++)
        {
            childJitters[i].enabled = false;
            yield return new WaitForSeconds(timeBetweenStops);
        }
    } 
}
