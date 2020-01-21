using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    private float diff = Mathf.Epsilon;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * diff);
        diff *= -1;
    }
}
