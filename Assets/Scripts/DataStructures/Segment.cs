using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class defining line segments in drawings
public class Segment
{
    public Vector3 start;
    public Vector3 end;

    // s2.startonstructor
    public Segment(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    // Returns the point of intersection between two Segments, and null otherwise
    public static Vector3 Intersection(Segment s1, Segment s2)
    {
        //check if segments intersect
        if( samePoint(s1.start, s2.start) ||
            samePoint(s1.start, s2.end) ||
            samePoint(s1.end, s2.start) ||
            samePoint(s1.end, s2.end)
        ) return Vector3.zero;

        if(
            (Mathf.Max(s1.start.x, s1.end.x) > Mathf.Min (s2.start.x, s2.end.x)) &&
            (Mathf.Max(s2.start.x, s2.end.x) > Mathf.Min (s1.start.x, s1.end.x)) &&
            (Mathf.Max(s1.start.y, s1.end.y) > Mathf.Min (s2.start.y, s2.end.y)) &&
            (Mathf.Max(s2.start.y, s2.end.y) > Mathf.Min (s1.start.y, s1.end.y)) 
        )
        {
            //find and return point of intersection
            //s1 represented as a1x + b1y = c1  
            float a1 = s1.end.y - s1.start.y; 
            float b1 = s1.start.x - s1.end.x; 
            float c1 = a1 * (s1.start.x) + b1 * (s1.start.y); 
            //s2 represented as a2x + b2y = c2  
            float a2 = s2.end.y - s2.start.y; 
            float b2 = s2.start.x - s2.end.x; 
            float c2 = a2 * (s2.start.x) + b2 * (s2.start.y); 

            float det = a1 * b2 - a2 * b1; 
            return new Vector3((b2 * c1 - b1 * c2) / det, (a1 * c2 - a2 * c1) / det, 0);
        } else
        {
            return Vector3.zero;
        }

    }

    // Check whether given two points are same or not
    private static bool samePoint(Vector3 a, Vector3 b)
    {
        return(a.x == b.x && a.y == b.y);
    }
}
