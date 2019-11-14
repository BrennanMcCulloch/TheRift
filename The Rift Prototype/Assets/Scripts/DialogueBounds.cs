using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBounds : MonoBehaviour
{
    public GameObject deadDimension;
    public DialogueTrigger inCaseOfDialogue;

    public Material materialBefore;
    public Material materialAfter;
    private Material gameMaterial;

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
        gameMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        deadDimension.transform.position += new Vector3(0f, 0f, 0.0000001f);

        if (collider.bounds.Contains(top) && collider.bounds.Contains(middle) && collider.bounds.Contains(bottom))
        {
            gameMaterial.color = materialAfter.color;
            //If it overlaps and you click on it 
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);
                if (hit.collider != null && hit.collider.gameObject.name == this.name)
                {
                    inCaseOfDialogue.TriggerDialogue();
                }
            }
        }
        else //it doesn't overlap
        {
            gameMaterial.color = materialBefore.color;
        }
    }
}
