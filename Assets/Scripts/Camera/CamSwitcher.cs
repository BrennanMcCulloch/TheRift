using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public Camera camOff;
    public Camera camOn;
    public UnityEvent thingsToTrigger;

   private void onTriggerEnter2D(Collider other)
    {
        Debug.Log("It's Triggering!");
        camOn.tag = "MainCamera";
        camOff.tag = "Untagged";
        thingsToTrigger.Invoke();
    }
}
