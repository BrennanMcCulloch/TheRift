using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public Camera camOff;
    public Camera camOn;
    public UnityEvent thingsToTrigger;

   private void OnTriggerEnter(Collider other)
    {
        Debug.Log("It's Triggering!");

        //Change the tag names for the cameras
        camOn.tag = "MainCamera";
        camOff.tag = "Untagged";

        //Swtich cameras on/off to toggle which camera the player sees
        camOn.enabled = true;
        camOff.enabled = false;

        thingsToTrigger.Invoke();
    }
}

