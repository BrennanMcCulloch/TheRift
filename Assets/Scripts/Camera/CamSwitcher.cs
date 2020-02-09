using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public GameObject camOn;

    // Switch cameras when the player goes here
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && camOn.name != Camera.main.gameObject.name)
        {
            SwitchTo(camOn);
        }
    }

    // Switch cameras and perform necessary adjustments to compensate
    public static void SwitchTo(GameObject newCamera)
    {
        GameObject oldCamera = Camera.main.gameObject;
        //play music on new camera at the same point as our old music was playing
        newCamera.GetComponent<AudioSource>().time = oldCamera.GetComponent<AudioSource>().time;
        oldCamera.SetActive(false);
        newCamera.SetActive(true);
        DrawManager.instance.Reposition();
    }
}