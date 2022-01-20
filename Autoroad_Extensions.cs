using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Autoroad_Extensions
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        degrees = -degrees;
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2 Abs(this Vector2 a)
    {
        return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
    }

    public static float DistanceToFoot(Vector2 a, Vector2 b, Vector2 o)
    {
        float x21 = b.x - a.x;
        float y21 = b.y - a.y;
        float x31 = o.x - a.x;
        float y31 = o.y - a.y;
        return (x31 * x21 + y31 * y21) / (x21 * x21 + y21 * y21);
    }

    public static Vector2 FootIntersectionPoint(Vector2 a, Vector2 b, Vector2 o)
    {
        return Vector2.Lerp(a, b, DistanceToFoot(a, b, o));
    }

    public static Vector2 OffsetByPoint(Vector2 x, Vector2 dir, Autoroad_Point point)
    {
        dir.Rotate(point.Rotation);
        return x + (dir * point.Offset);
    }
}