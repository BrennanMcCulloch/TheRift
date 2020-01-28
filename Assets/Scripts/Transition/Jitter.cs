using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    public float maximumDisplacement;
    private Vector3 originalPosition;
    private Vector3 lowerBound;
    private Vector3 upperBound;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        lowerBound = new Vector3(originalPosition.x - maximumDisplacement,
                                 originalPosition.y - maximumDisplacement,
                                 originalPosition.z - maximumDisplacement
                                );
        upperBound = new Vector3(originalPosition.x + maximumDisplacement,
                                 originalPosition.y + maximumDisplacement,
                                 originalPosition.z + maximumDisplacement
                                );
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Random.Range(lowerBound.x, upperBound.x),
                                         Random.Range(lowerBound.y, upperBound.y),
                                         Random.Range(lowerBound.z, upperBound.z)
                                        );
    }

    // switch to the original position
    void OnDisable()
    {
        transform.position = originalPosition;
    }
}
