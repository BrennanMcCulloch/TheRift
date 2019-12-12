//Stolen Online

using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {
    
    public GameObject objectToFollow;
    
    public float speed = 2.0f;

    public float verticalOffset = 1.0f;
    
    void Update () {
        float interpolation = speed * Time.deltaTime;
        
        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + verticalOffset, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
        
        this.transform.position = position;
    }
}