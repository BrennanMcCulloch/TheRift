using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    //singleton
    public static CreateMesh instance;
    //offsetting the position of rift
    public float camDis = 5f;
    //public float xOffset;
    //public float yOffset;

    static private Mesh mesh;
    public bool firstHeld;

    private float distance;

    private Plane planeObj;
    private Vector3 startPos;

    // Awake is called once before start
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        planeObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);
        firstHeld = true;
    }

    // Create polygon collider connecting points from one index to another of a list
    public static void Create(int start, int end, List<Vector3> points)
    {
        List<Vector3> newVertices = points.GetRange(start, points.Count - start - (points.Count - end));
        //adjust points for camera and add new ones for the back face
        int halfCount = newVertices.Count;
        for (int i = 0; i < halfCount; i++)
        {
            newVertices[i] = new Vector3(newVertices[i].x, newVertices[i].y, instance.camDis);
            newVertices.Add(newVertices[i] + new Vector3(0, 0, 10));
        }
        //set triangles for mesh
        List<int> newTriangles = new List<int>();
        //draw front with normals facing both ways for visibility
        //front circle cw
        for (int i = 1; i < (halfCount) - 1; i++)
        {
            newTriangles.Add(0);
            newTriangles.Add(i);
            newTriangles.Add(i + 1);
        }
        //front circle cc
        for (int i = 1; i < halfCount - 1; i++)
        {
            newTriangles.Add(i + 1);
            newTriangles.Add(i);
            newTriangles.Add(0);
        }

        //back circle cw
        for (int i = (halfCount) + 1; i < newVertices.Count - 1; i++)
        {
            newTriangles.Add(halfCount);
            newTriangles.Add(i);
            newTriangles.Add(i + 1);
        }

        //body cw
        int frontVertex = 0;
        int backVertex = halfCount;
        while (frontVertex < halfCount - 1 && backVertex < newVertices.Count)
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
        newTriangles.Add((halfCount) - 1);
        newTriangles.Add(newVertices.Count - 1);
        newTriangles.Add(0);
        newTriangles.Add(newVertices.Count - 1);
        newTriangles.Add(halfCount);
        newTriangles.Add(0);

        mesh = new Mesh();
        instance.GetComponent<MeshFilter>().mesh = mesh;
        instance.GetComponent<MeshCollider>().sharedMesh = mesh;
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
    }
}