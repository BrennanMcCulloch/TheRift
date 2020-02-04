using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public GameObject camOff;
    public GameObject camOn;

    private void OnTriggerEnter(Collider other)
    {
        camOn.SetActive(true);
        camOff.SetActive(false);
        //reset things which rely on camera position
        DrawManager.instance.Reposition();
    }
}