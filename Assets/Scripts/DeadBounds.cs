using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBounds : MonoBehaviour
{
    public GameObject toggleA;//set this active
    public GameObject toggleB;//set this inactive

    private Collider itemColliding;
    private Vector3 top;
    private Vector3 middle;
    private Vector3 bottom;

    // Start is called before the first frame update
    void Start()
    {
        itemColliding = this.GetComponent<Collider>();
        middle = itemColliding.bounds.center;
        Vector3 scale = itemColliding.bounds.extents;
        top = new Vector3(middle.x + scale.x, middle.y + scale.y, middle.z + scale.z);
        bottom = new Vector3(middle.x - scale.x, middle.y - scale.y, middle.z - scale.z);
    }

    // Update is called once per frame
    void Update()
    {
        //deadDimension.transform.position += new Vector3(0f, 0f, 0.0000001f);
    }

    // If this collides with DeadDimension, turn it "on"
    void OnTriggerEnter (Collider other)
    {
        Debug.Log("seeing");//test
        if (other.gameObject.tag == "DeadDimension")
        {
            Debug.Log("believing");//test
            if (other.bounds.Contains(top) &&
                other.bounds.Contains(middle) &&
                other.bounds.Contains(bottom))
            {
                Debug.Log("all in");//test
                RiftMeshManager.AddDeadObject(this);
                if(toggleA != null)
                {
                    toggleA.SetActive(true);
                }
                if(toggleB != null)
                {
                    toggleB.SetActive(false);
                }
            }
        }
    }

    //This is essentially works like "OnTriggerExit" for when the rift is redrawn
    public void DeadOutOfBounds ()
    {
        if (toggleA != null)
        {
            toggleA.SetActive(false);
        }
        if (toggleB != null)
        {
            toggleB.SetActive(true);
        }
    }
}
