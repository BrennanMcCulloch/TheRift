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

    private static Mesh mesh;
    private static List<Vector3> meshPoints;
    private static CircularList<int> activeIndices;
    private static List<int> convexIndices;
    private static bool[] convexBool;
    private static List<int> reflexIndices;
    private static List<int> earIndices;
    private static bool flipConvex;
    public bool firstHeld;

    private Plane planeObj;
    private Vector3 startPos;
    private float distance;

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
        //limit mesh points to those within the closed polygon
        meshPoints = points.GetRange(start, points.Count - start - (points.Count - end));
        //adjust points for camera and add new ones for the back face
        int halfCount = meshPoints.Count;
        for (int i = 0; i < halfCount; i++)
        {
            meshPoints[i] = new Vector3(meshPoints[i].x, meshPoints[i].y, instance.camDis);
            meshPoints.Add(meshPoints[i] + new Vector3(0, 0, 10));
        }
        //fill the list of point indices not yet fully handled
        int[] allIndices = new int[halfCount];
        for (int i = 0; i < allIndices.Length; i++)
        {
            allIndices[i] = i;
        }
        activeIndices = new CircularList<int>(allIndices);
        //find reflex/ convex points
        flipConvex = false;
        convexIndices = new List<int>();
        reflexIndices = new List<int>();
        convexBool = new bool[halfCount];
        for (int i = 0; i < activeIndices.Count; i++)
        {
            SortPoint(i);
        }
        //if our sort yeilded more reflex that convex, we have to flip our terms
        if (convexIndices.Count < reflexIndices.Count)
        {
            flipConvex = true;
            convexIndices = new List<int>();
            reflexIndices = new List<int>();
            convexBool = new bool[halfCount];
            for (int i = 0; i < activeIndices.Count; i++)
            {
                SortPoint(i);
            }
        }
        //find ears
        earIndices = new List<int>();
        foreach (int i in convexIndices)
        {
            if (IsEar(i)) earIndices.Add(i);
        }
        //buffer/remove ears one at a time
        List<int> meshIndices = new List<int>();
        while (activeIndices.Count > 2 && earIndices.Count > 0)
        {
            int i = earIndices[0];
            //get neighboring indices
            int prev = activeIndices.ValueBefore(i);
            int next = activeIndices.ValueAfter(i);
            //draw front face both directions
            meshIndices.Add(i);
            meshIndices.Add(prev);
            meshIndices.Add(next);
            meshIndices.Add(i);
            meshIndices.Add(next);
            meshIndices.Add(prev);
            //remove ear
            activeIndices.Remove(i);
            earIndices.Remove(i);
            //check new status of adjacent points
            ReSortPoint(prev);
            ReSortPoint(next);
            if (convexBool[next] == true && !earIndices.Contains(next) && IsEar(next)) earIndices.Add(next);
        }
        //draw body connecting the two faces
        int frontVertex = 0;
        int backVertex = halfCount;
        while (frontVertex < halfCount - 1 && backVertex < meshPoints.Count)
        {
            meshIndices.Add(frontVertex);
            meshIndices.Add(backVertex);
            meshIndices.Add(backVertex + 1);
            meshIndices.Add(frontVertex);
            meshIndices.Add(backVertex + 1);
            meshIndices.Add(frontVertex + 1);
            frontVertex++;
            backVertex++;
        }
        meshIndices.Add((halfCount) - 1);
        meshIndices.Add(meshPoints.Count - 1);
        meshIndices.Add(0);
        meshIndices.Add(meshPoints.Count - 1);
        meshIndices.Add(halfCount);
        meshIndices.Add(0);
        //load in the new mesh
        mesh = new Mesh();
        instance.GetComponent<MeshFilter>().mesh = mesh;
        instance.GetComponent<MeshCollider>().sharedMesh = mesh;
        mesh.vertices = meshPoints.ToArray();
        mesh.triangles = meshIndices.ToArray();
    }

    //Sorts the index into the convex or reflex list
    private static void SortPoint(int index)
    {
        if (GetTriangleFor(index).Convex())
        {
            convexBool[index] = true;
            convexIndices.Add(index);
        } else
        {
            convexBool[index] = false;
            reflexIndices.Add(index);
        }
    }

    //Used to properly identify convex/ear points after their neighbors are removed
    private static void ReSortPoint(int index)
    {
        if (GetTriangleFor(index).Convex() && convexBool[index] == false)
        {
            convexBool[index] = true;
            convexIndices.Add(index);
            if (!earIndices.Contains(index) && IsEar(index))
            {
                earIndices.Add(index);
                return;
            }
        }
        if (earIndices.Contains(index) && !IsEar(index))
        {
            earIndices.Remove(index);
            return;
        }
    }

    //Returns true if no other active point is inside the triangle
    private static bool IsEar(int index)
    {
        Triangle tri = GetTriangleFor(index);
        foreach(int reflex in reflexIndices)
        {
            if (tri.Contains(meshPoints[reflex]))
            {
                return false;
            }
        }
        return true;
    }

    //Returns the triangle centered around a given index
    private static Triangle GetTriangleFor(int index)
    {
        //make triangle in opposite order depending on user drawing direction
        if (flipConvex == true)
        {
            return new Triangle(GetActivePointAt(index),
                                GetActivePointAfter(index),
                                GetActivePointBefore(index)
                               );
        } else
        {
            return new Triangle(GetActivePointAt(index),
                                GetActivePointBefore(index),
                                GetActivePointAfter(index)
                               );
        }
    }

    //Returns the point represented by an active index
    private static Vector3 GetActivePointBefore(int index)
    {
        return meshPoints[activeIndices.ValueBefore(index)];
    }

    //Returns the point represented by an active index
    private static Vector3 GetActivePointAt(int index)
    {
        return meshPoints[index];
    }

    //Returns the point represented by an active index
    private static Vector3 GetActivePointAfter(int index)
    {
        return meshPoints[activeIndices.ValueAfter(index)];
    }
}