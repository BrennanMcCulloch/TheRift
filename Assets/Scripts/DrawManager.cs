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
    private List<Segment> segments;
    private Vector3 startPos;
    private float distance;

    // Awake is called once before start
    void Awake()
    {
        instance = this;
        points = new List<Vector3>();
        segments = new List<Segment>();
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
            points.Clear();
            segments.Clear();
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
        //if this is not the fist point, add a segment and check collision
        if(points.Count > 1)
        {
            segments.Add(new Segment(points[points.Count - 2], point));
            //check for collision
            for(int i = 0; i < segments.Count - 1; i++)
            {
                if(segments[segments.Count - 1].intersects(segments[i]))
                {
                    Debug.Log("Is collide");
                } else
                {
                    Debug.Log("Not collide");
                }
            }
        }
    }
}
