using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBounds : MonoBehaviour
{
    public GameObject deadDimension;
    public GameObject toggleA;//set this active
    public GameObject toggleB;//set this inactive

    private MeshCollider collider;
    private Collider itemColliding;

    private Vector3 top;
    private Vector3 middle;
    private Vector3 bottom;

    // Start is called before the first frame update
    void Start()
    {
        collider = deadDimension.GetComponent<MeshCollider>();
        itemColliding = this.GetComponent<Collider>();
        middle = itemColliding.bounds.center;
        Vector3 scale = itemColliding.bounds.extents;
        top = new Vector3(middle.x + scale.x, middle.y + scale.y, middle.z + scale.z);
        bottom = new Vector3(middle.x - scale.x, middle.y - scale.y, middle.z - scale.z);
    }

    // Update is called once per frame
    void Update()
    {
        deadDimension.transform.position += new Vector3(deadDimension.transform.position.x, deadDimension.transform.position.y, 0.0000001f);

        if (collider.bounds.Contains(top) && collider.bounds.Contains(middle) && collider.bounds.Contains(bottom))
        {
            toggleA.SetActive(true);
            toggleB.SetActive(false);
        }
        else
        {
            toggleA.SetActive(false);
            toggleB.SetActive(true);
        }

    }
}
