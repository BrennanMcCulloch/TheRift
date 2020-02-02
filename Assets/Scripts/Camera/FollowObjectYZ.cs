//Stolen Online

using UnityEngine;
using System.Collections;

public class FollowObjectYZ : MonoBehaviour {
    
    public GameObject objectToFollow;
    
    public float speed = 2.0f;

    public float verticalOffset = 1.0f;
    
    void Update () {
        float interpolation = speed * Time.deltaTime;
        
        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + verticalOffset, interpolation);
        position.z = Mathf.Lerp(this.transform.position.z, objectToFollow.transform.position.z, interpolation);
        
        this.transform.position = position;
    }
}