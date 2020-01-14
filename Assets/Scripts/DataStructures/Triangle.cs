using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    private Vector3 a;
    private Vector3 b;
    private Vector3 c;

    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    //Returns the area between three points
    public static float Area(Vector3 a, Vector3 b, Vector3 c)
    {
        float area = 0;
        area += a.x * (c.y - b.y);
        area += b.x * (a.y - c.y);
        area += c.x * (b.y - a.y);
        return area / 2;
    }

    //Returns true if point is inside this
    public bool Contains(Vector3 point)
    {
        //find area of triangles formed by point with a, b and c
        float area1 = Area(point, a, b);
        float area2 = Area(point, b, c);
        float area3 = Area(point, c, a);
        bool has_neg = (area1 < 0) || (area2 < 0) || (area3 < 0);
        bool has_pos = (area1 > 0) || (area2 > 0) || (area3 > 0);
        return (area1 + area2 + area3 < Area(a, b, c) - 0.5);
    }

    //Returns true if the point at the index is convex (counter clockwise)
    public bool Convex()
    {
        if (Area(a, b, c) <= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

}