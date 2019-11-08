using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    //offsetting the position of rift
    public float camDis = 5f;
    public float xOffset;
    public float yOffset;

    private List<Vector3> newVertices;
    private List<int> newTriangles;
    private Mesh mesh;

    private float distance;

    private Plane planeObj;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        planeObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //On mouse down
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            newVertices = new List<Vector3>();
            newTriangles = new List<int>();

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (planeObj.Raycast(mouseRay, out distance))
            {
                startPos = mouseRay.GetPoint(distance);
                Vector3 temp = new Vector3(mouseRay.GetPoint(distance).x + xOffset, mouseRay.GetPoint(distance).y + yOffset, camDis);
                newVertices.Add(temp);
            }
        }

        //On mouse held & moved. Getting rid of touch made it work. NEED TO REIMPLEMENT.
        else if (Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance) != startPos && Input.GetMouseButton(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (planeObj.Raycast(mouseRay, out distance))
            {
                Vector3 temp = new Vector3(mouseRay.GetPoint(distance).x + xOffset, mouseRay.GetPoint(distance).y + yOffset, camDis);
                newVertices.Add(temp);
            }
        }

        //On mouse up. Bound to need to use this.
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
            //If you drew and didn't just click
            if(newVertices.Count > 2)
            {
                //they drew it clockwise!
                if (newVertices[0].x < newVertices[1].x)
                {
                    for (int i = 1; i < newVertices.Count - 1; i++)
                    {
                        newTriangles.Add(0);
                        newTriangles.Add(i);
                        newTriangles.Add(i + 1);
                    }
                }
                else //they drew it counter clockwise!
                {
                    for (int i = 1; i < newVertices.Count - 1; i++)
                    {
                        newTriangles.Add(i + 1);
                        newTriangles.Add(i);
                        newTriangles.Add(0);
                    }
                }

                mesh.vertices = newVertices.ToArray();
                mesh.triangles = newTriangles.ToArray();
            }


        }
    }
}
