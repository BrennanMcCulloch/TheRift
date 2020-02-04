using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public GameObject camOff;
    public GameObject camOn;
    public UnityEvent thingsToTrigger;

    // Switch cameras when the player goes here
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SwitchTo(camOn, camOff);
        }
    }

    // Switch cameras and perform necessary adjustments to compensate
    public static void SwitchTo(GameObject newCamera, GameObject oldCamera)
    {
        newCamera.SetActive(true);
        oldCamera.SetActive(false);
        DrawManager.instance.Reposition();
    }
}