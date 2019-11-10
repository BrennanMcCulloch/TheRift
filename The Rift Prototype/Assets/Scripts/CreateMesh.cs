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
    public bool firstHeld;

    private float distance;

    private Plane planeObj;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        planeObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);
        firstHeld = true;
    }
        
    // Update is called once per frame
    void Update()
    {
        //Need to round current Mouse variables for comparison because FUCK FLOATS
        var currentRoundX = System.Math.Round(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance).x, 2);
        var currentRoundY = System.Math.Round(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance).y, 2);

        //On mouse down
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            newVertices = new List<Vector3>();
            newTriangles = new List<int>();

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (planeObj.Raycast(mouseRay, out distance))
            {
                startPos = mouseRay.GetPoint(distance);
                Vector3 temporary = new Vector3(startPos.x + xOffset, startPos.y + yOffset, camDis);
                newVertices.Add(temporary);
            }
        }

        //On mouse held & moved. Getting rid of touch made it work. NEED TO REIMPLEMENT.
        else if ((currentRoundX != System.Math.Round(startPos.x, 2)) && (currentRoundY != System.Math.Round(startPos.y, 2)) && Input.GetMouseButton(0))
        {
            if (firstHeld == true)
            {
                mesh = new Mesh();
                GetComponent<MeshFilter>().mesh = mesh;
                GetComponent<MeshCollider>().sharedMesh = mesh;
            }
            firstHeld = false;

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
            if (newVertices.Count > 2)
            {
                firstHeld = true;
                //Make the second set of vertices behind the current set so there's a shape
                int dontKeepThis = newVertices.Count;
                for (int i = 0; i < dontKeepThis; i++)
                {
                    Vector3 temp = new Vector3(newVertices[i].x, newVertices[i].y, newVertices[i].z + 10);
                    newVertices.Add(temp);
                }

                //they drew it clockwise!
                if (newVertices[0].x < newVertices[1].x)
                {
                    //front circle
                    for (int i = 1; i < (newVertices.Count / 2) - 1; i++)
                    {
                        newTriangles.Add(0);
                        newTriangles.Add(i);
                        newTriangles.Add(i + 1);
                    }

                    //back circle
                    for (int i = (newVertices.Count / 2) + 1; i < newVertices.Count - 1; i++)
                    {
                        newTriangles.Add(newVertices.Count / 2);
                        newTriangles.Add(i);
                        newTriangles.Add(i + 1);
                    }

                    //body
                    int frontVertex = 0;
                    int backVertex = (newVertices.Count / 2);
                    while (frontVertex < (newVertices.Count / 2) - 1 && backVertex < newVertices.Count)
                    {
                        newTriangles.Add(frontVertex);
                        newTriangles.Add(backVertex);
                        newTriangles.Add(backVertex + 1);
                        newTriangles.Add(frontVertex);
                        newTriangles.Add(backVertex + 1);
                        newTriangles.Add(frontVertex + 1);
                        frontVertex++;
                        backVertex++;
                    }
                    newTriangles.Add((newVertices.Count / 2) - 1);
                    newTriangles.Add(newVertices.Count - 1);
                    newTriangles.Add(0);
                    newTriangles.Add(newVertices.Count - 1);
                    newTriangles.Add(newVertices.Count / 2);
                    newTriangles.Add(0);
                }
                else //they drew it counter clockwise!
                {
                    //front circle
                    for (int i = 1; i < (newVertices.Count / 2) - 1; i++)
                    {
                        newTriangles.Add(i + 1);
                        newTriangles.Add(i);
                        newTriangles.Add(0);
                    }

                    //back circle
                    for (int i = (newVertices.Count / 2) + 1; i < newVertices.Count - 1; i++)
                    {
                        newTriangles.Add(i + 1);
                        newTriangles.Add(i);
                        newTriangles.Add(newVertices.Count / 2);
                    }

                    //body
                    int frontVertex = 0;
                    int backVertex = (newVertices.Count / 2);
                    while (frontVertex < (newVertices.Count / 2) - 1 && backVertex < newVertices.Count)
                    {
                        newTriangles.Add(backVertex + 1);
                        newTriangles.Add(backVertex);
                        newTriangles.Add(frontVertex);
                        newTriangles.Add(frontVertex + 1);
                        newTriangles.Add(backVertex + 1);
                        newTriangles.Add(frontVertex);
                        frontVertex++;
                        backVertex++;
                    }
                    newTriangles.Add(0);
                    newTriangles.Add(newVertices.Count - 1);
                    newTriangles.Add((newVertices.Count / 2) - 1);
                    newTriangles.Add(0);
                    newTriangles.Add(newVertices.Count / 2);
                    newTriangles.Add(newVertices.Count - 1);
                }

                mesh.vertices = newVertices.ToArray();
                mesh.triangles = newTriangles.ToArray();
            }


        }
    }
}
