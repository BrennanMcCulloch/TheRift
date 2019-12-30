using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class defining line segments in drawings
public class Segment
{
    public Vector3 start;
    public Vector3 end;

    // Constructor
    public Segment(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    // Checks whether a given Segment intersects with this
    public bool intersects(Segment other)
    {
        if( samePoint(this.start, other.start) ||
            samePoint(this.start, other.end) ||
            samePoint(this.end, other.start) ||
            samePoint(this.end, other.end)
        ) return false;
        
        return(
            (Mathf.Max(this.start.x, this.end.x) >= Mathf.Min (other.start.x, other.end.x)) &&
            (Mathf.Max(other.start.x, other.end.x) >= Mathf.Min (this.start.x, this.end.x)) &&
            (Mathf.Max(this.start.y, this.end.y) >= Mathf.Min (other.start.y, other.end.y)) &&
            (Mathf.Max(other.start.y, other.end.y) >= Mathf.Min (this.start.y, this.end.y)) 
        );
    }

    // Check whether given two points are same or not
    private bool samePoint(Vector3 pointA, Vector3 pointB)
    {
        return(pointA.x == pointB.x && pointA.y == pointB.y);
    }
}
