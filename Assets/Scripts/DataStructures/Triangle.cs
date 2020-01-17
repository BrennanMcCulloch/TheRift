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
    public bool Contains(Vector3 p)
    {
        bool isWithinTriangle = false;
        //based on Barycentric coordinates
        float denominator = ((b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y));

        float ba = ((b.y - c.y) * (p.x - c.x) + (c.x - b.x) * (p.y - c.y)) / denominator;
        float bb = ((c.y - a.y) * (p.x - c.x) + (a.x - c.x) * (p.y - c.y)) / denominator;
        float bc = 1 - ba - bb;

        //the point is within the triangle
        if (ba > 0f && ba < 1f && bb > 0f && bb < 1f && bc > 0f && bc < 1f)
        {
            isWithinTriangle = true;
        }

        return isWithinTriangle;
    }

    //Returns true if the point at the index is convex (counter clockwise)
    public bool Convex()
    {
        float det = a.x * b.y + c.x * a.y + b.x * c.y - a.x * c.y - c.x * b.y - b.x * a.y;
        return det <= 0f;
    }

}