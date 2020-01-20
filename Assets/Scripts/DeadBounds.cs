using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBounds : MonoBehaviour
{
    public GameObject toggleA;//set this active
    public GameObject toggleB;//set this inactive
    public AudioClip narrationClip;

    private Collider itemColliding;
    private Vector3 top;
    private Vector3 middle;
    private Vector3 bottom;
    private bool narrationReady = false;

    // Start is called before the first frame update
    void Start()
    {
        itemColliding = this.GetComponent<Collider>();
        middle = itemColliding.bounds.center;
        Vector3 scale = itemColliding.bounds.extents;
        top = new Vector3(middle.x + scale.x, middle.y + scale.y, middle.z + scale.z);
        bottom = new Vector3(middle.x - scale.x, middle.y - scale.y, middle.z - scale.z);
        //if we have narration, we can read it later
        if (narrationClip != null)
        {
            narrationReady = true;
        }
    }

    // If this collides with DeadDimension, turn it "on" and play any narration
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "DeadDimension")
        {
            if (other.bounds.Contains(top) &&
                other.bounds.Contains(middle) &&
                other.bounds.Contains(bottom))
            {
                //let the rift know we need to know when it's gone
                RiftMeshManager.AddDeadObject(this);
                //read the narration once
                if(narrationReady == true)
                {
                    Narration.narrate(narrationClip);
                    narrationReady = false;
                }
                //make changes to the scene in response to this being revealed
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
