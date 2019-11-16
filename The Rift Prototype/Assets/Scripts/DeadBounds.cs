using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBounds : MonoBehaviour
{
    public GameObject deadDimension;
    public GameObject toggleA;//set this active
    public GameObject toggleB;//set this inactive

    private Collider collider;
    private Collider itemColliding;

    private Vector3 top;
    private Vector3 middle;
    private Vector3 bottom;

    // Start is called before the first frame update
    void Start()
    {
        collider = deadDimension.GetComponent<Collider>();
        itemColliding = this.GetComponent<Collider>();
        middle = itemColliding.bounds.center;
        Vector3 scale = itemColliding.bounds.extents;
        top = new Vector3(middle.x + scale.x, middle.y + scale.y, middle.z + scale.z);
        bottom = new Vector3(middle.x - scale.x, middle.y - scale.y, middle.z - scale.z);
    }

    // Update is called once per frame
    void Update()
    {
        deadDimension.transform.position += new Vector3(0f, 0f, 0.0000001f);

        if (collider.bounds.Contains(top) && collider.bounds.Contains(middle) && collider.bounds.Contains(bottom))
        {
            if(toggleA != null)
            {
                toggleA.SetActive(true);
            }
            if(toggleB != null)
            {
                toggleB.SetActive(false);
            }
        }
        else
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
}
