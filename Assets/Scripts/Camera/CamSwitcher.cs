﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamSwitcher : MonoBehaviour
{
    public GameObject camOff;
    public GameObject camOn;
    public UnityEvent thingsToTrigger;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("It's Triggering!");

        camOn.SetActive(true);
        camOff.SetActive(false);

        thingsToTrigger.Invoke();
    }
}