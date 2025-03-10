﻿//Stole this online
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : Singleton<DrawManager>
{
    // minimum distance must be dragged before drawing kicks in. helps separate player nav and drawing actions
    const float MINIMUM_DRAW_DISTANCE = 50.0f;
    //When something is drawn, we make planes. Trail render.
    public GameObject drawPrefab;
    // Track where we started touching/dragging. Used to calculate deadzone
    private Vector3 startedHoldingPosition;
    private GameObject theTrail;
    private Plane planeObj;
    private List<Vector3> points;
    private List<Segment> segments;
    private float distance;
    private int loopStart = 0;
    private int loopEnd = 0;
    private bool drawing;

    // Awake is called once before start
    void Awake()
    {
        points = new List<Vector3>();
        segments = new List<Segment>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reposition();
    }

	// Add event handlers
	void OnEnable()
    {
		InputManager.OnPress += OnPress;
        InputManager.OnHold += OnHold;
        InputManager.OnRelease += OnRelease;
	}

	// Remove event handlers
	void OnDisable()
    {
		InputManager.OnPress -= OnPress;
        InputManager.OnHold -= OnHold;
        InputManager.OnRelease -= OnRelease;
	}

    // Make a note of where we started drawing.
    void OnPress() {
        startedHoldingPosition = Input.mousePosition;
    }

    // Use a deadzone for draw detection so we can separate drawing actions from navigation actions
    void OnHold() {
        // Bail unless we're already drawing, or we're too close to the start position.
        if (!drawing && !DraggedPastThreshold(startedHoldingPosition)) return;

        if (drawing) {
            DrawTrail();
        }
        else {
            StartTrail();
            drawing = true;
        }
    }

    // Tries to open a rift when we stop drawing
    void OnRelease() {
        //create mesh if there was a collision
        if(loopEnd - loopStart > 5)
        {
            RiftMeshManager.Create(loopStart, loopEnd, points);
        }
        //destroy the game object 6 seconds after drawing
        Destroy(theTrail, 6);
        ClearTrail();
        drawing = false;
    }

    // Executed every frame while the user is touching/dragging and defines the shape of our line via points
    void DrawTrail() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(planeObj.Raycast(mouseRay, out distance))
        {
            Vector3 point = mouseRay.GetPoint(distance);
            if(!points.Contains(point))
            {
                AddPoint(point);
            }
        }
    }

    // Cleans up all the trail elements before drawing a new trail
    void ClearTrail() {
        Destroy(theTrail);//test
        points.Clear();
        segments.Clear();
        loopEnd = 0;
        loopStart = 0;
    }

    // Refreshes positioning data and starts a new trail at the current mousePosition-to-screenPoint.
    void StartTrail() {
        this.transform.position = startedHoldingPosition;
        theTrail = (GameObject)Instantiate(drawPrefab, this.transform.position, Quaternion.identity);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(planeObj.Raycast(mouseRay, out distance))
        {
            Vector3 point = mouseRay.GetPoint(distance);
            AddPoint(point);
        }
    }

    // Adds a point and handles what to do if a rift was drawn
    private void AddPoint(Vector3 point) {
        //show point on trail
        theTrail.transform.position = point;
        //maintain points in 2d for easy processing
        point = Camera.main.WorldToScreenPoint(point);
        //add point
        points.Add(point);
        //if this is not the fist point, add a segment and check for a loop
        if(points.Count > 1)
        {
            //check for direct intersection
            for (int i = 0; i < points.Count - 10; i++)
            {
                if (Vector3.Distance(point, points[i]) < 0.15f)
                {
                    HandleLoop(i, point);
                    return;
                }
            }
            //check for line crossing
            segments.Add(new Segment(points[points.Count - 2], point));
            for(int i = 0; i < segments.Count - 1; i++)
            {
                Vector3 intersection = Segment.Intersection(segments[segments.Count - 1], segments[i]);
                if(intersection != Vector3.zero && loopEnd == 0)
                {
                    HandleLoop(i, intersection);
                }
            }
        }
    }

    //Sets variables to represent the created loop if it is big enough
    private void HandleLoop(int index, Vector3 intersection)
    {
        if(points.Count - index > 7)
        {
            //set points to encapsulate the loop created
            loopStart = index + 1;
            points[points.Count - 1] = intersection;
            loopEnd = points.Count - 1;
            return;
        }
    }

    // Determine whether a given Vector3 is far enough away from the starting Vector3
    bool DraggedPastThreshold(Vector3 startingPosition) {
        return Vector3.Distance(Input.mousePosition, startingPosition) > MINIMUM_DRAW_DISTANCE;
    }

    // Set objects position based on camera, then get the plane on which to draw from that
    public void Reposition()
    {
        transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
        transform.Translate(Camera.main.transform.forward * 2);
        planeObj = new Plane(Camera.main.transform.forward, this.transform.position);
        RiftMeshManager.instance.Reposition();
    }
}
