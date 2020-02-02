using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public Camera camMain;
    public Camera camBar;
    public UnityEvent thingsToTrigger;

   private void OnTriggerEnter(Collider other)
    {
        Debug.Log("It's Triggering!");

        //Change the tag names for the cameras
        camBar.tag = "MainCamera";
        camMain.tag = "Untagged";

        //Swtich cameras on/off to toggle which camera the player sees
        camBar.enabled = true;
        camMain.enabled = false;

        thingsToTrigger.Invoke();
    }
}

