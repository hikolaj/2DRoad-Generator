using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoroad_Gizmo : MonoBehaviour
{
    [HideInInspector] public Autoroad Autoroad;

    public void Start()
    {
        Autoroad = GetComponent<Autoroad>();
    }

    public void OnDrawGizmos()
    {
        if (Autoroad != null && Autoroad.PathGenerator != null && Autoroad.PathGenerator.Path.Count > 0)
        {
            float closestDistance = 0; 
            float dis = Autoroad.PathGenerator.CalculateDistanceAtPosition(Autoroad.Car.position);
            Autoroad_Point point = Autoroad.PathGenerator.Path[0];
            Vector2 dir = Autoroad.transform.up;
            Vector2 o = Autoroad.Car.position;
            Vector2 a = Autoroad.transform.position;
            Vector2 b = a;
            Vector2 foot = a;

            for (int i = 0; i < Autoroad.PathGenerator.Path.Count; i++)
            {
                point = Autoroad.PathGenerator.Path[i];
                dir = dir.Rotate(point.Rotation);
                b = a + (dir * point.Offset);

                if (Vector2.Dot((o - b).normalized, dir) < 0)
                {
                    foot = Autoroad_Extensions.FootIntersectionPoint(a, b, o);// N
                    closestDistance += Vector2.Distance(a, foot);
                    break;
                }
                else
                {
                    a = b;
                    closestDistance += point.Offset;
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(o, 0.3f);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(a, 0.2f);
            Gizmos.DrawSphere(b, 0.2f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(a, o);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(o, b);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(o, foot);
            Gizmos.DrawSphere(foot, 0.4f);
        }
        
    }
}
