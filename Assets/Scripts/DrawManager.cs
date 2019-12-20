//Stole this online
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    //This makes the class act as a singleton
    public static DrawManager instance;

    //When something is drawn, we make planes. Trail render. 
    public GameObject drawPrefab;

    //private LineRenderer line;
    private GameObject theTrail;
    private Plane planeObj;
    private List<Vector3> points;
    private Vector3 startPos;
    private float distance;

    // Structure for line segments
    struct segment
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
    };

    // Awake is called once before start
    void Awake()
    {
        instance = this;
        points = new List<Vector3> ();
    }

    // Start is called before the first frame update
    void Start()
    {
        planeObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            //formerly FixDrawStart
            Vector3 temp = Input.mousePosition;
            temp.z = 5f;
            this.transform.position = Camera.main.ScreenToWorldPoint(temp);
            //replace/create trail
            Destroy(theTrail);
            theTrail = (GameObject)Instantiate(drawPrefab, this.transform.position, Quaternion.identity);
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(planeObj.Raycast(mouseRay, out distance))
            {
                Vector3 point = mouseRay.GetPoint(distance);
                startPos = point;
                addPoint(point);
            }
        }
        else if(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance) != startPos && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(planeObj.Raycast(mouseRay, out distance))
            {
                Vector3 point = mouseRay.GetPoint(distance);
                if(!points.Contains(point))
                {
                    addPoint(point);
                }
            }
        }

        //On mouse up. Bound to need to use this.
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
            //destroy the game object 6 seconds after drawing
            Destroy(theTrail, 6);
        }
    }

    // Adds a point and handles what to do if a rift is drawn
    private void addPoint(Vector3 point) {
        //add point
        points.Add(point);
        theTrail.transform.position = point;
        //check for collision
        if(isLineCollide())
        {
            Debug.Log("Is collide");
        } else
        {
            Debug.Log("Not collide");
        }
    }

    // Checks if currentLine (line drawn by last two points) collided with line   
    private bool isLineCollide()
    {
        if(points.Count < 2) return false;
        int TotalLines = points.Count - 1;
        segment[] lines = new segment[TotalLines];
        if(TotalLines > 1)
        {
            for(int i=0; i<TotalLines; i++)
            {
                lines [i].StartPoint = (Vector3)points [i];
                lines [i].EndPoint = (Vector3)points [i + 1];
            }
        }
        for(int i=0; i<TotalLines-1; i++)
        {
            segment currentLine;
            currentLine.StartPoint = (Vector3)points [points.Count - 2];
            currentLine.EndPoint = (Vector3)points [points.Count - 1];
            if(isLinesIntersect (lines [i], currentLine)) return true;
        }
        return false;
    }
  
    // Check whether given two points are same or not  
    private bool checkPoints(Vector3 pointA, Vector3 pointB)
    {
        return(pointA.x == pointB.x && pointA.y == pointB.y);
    }
 
    // Checks whether given two line intersect or not 
    private bool isLinesIntersect(segment L1, segment L2)
    {
        if( checkPoints(L1.StartPoint, L2.StartPoint) ||
            checkPoints(L1.StartPoint, L2.EndPoint) ||
            checkPoints(L1.EndPoint, L2.StartPoint) ||
            checkPoints(L1.EndPoint, L2.EndPoint)
        ) return false;
        
        return(
            (Mathf.Max(L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
            (Mathf.Max(L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
            (Mathf.Max(L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
            (Mathf.Max(L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y)) 
        );
    }
}
