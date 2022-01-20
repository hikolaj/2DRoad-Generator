using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoroad : MonoBehaviour
{
    public float PlayerDistance { get; private set; }

    public Transform Car;
    public float Width = 10;
    public float RoadsideWidth = 20;
    public int Length = 50;
    public float StartOffset = 20;

    public Autoroad_PathGeneratorSettings PathSettings;
    public Autoroad_PathGenerator PathGenerator;
    public Autoroad_MeshGenerator MeshGenerator;
    //public Autoroad_ObstacleGenerator ObstacleGenerator;

    void Start()
    {
        PathGenerator = new Autoroad_PathGenerator(Length, transform);
        MeshGenerator = new Autoroad_MeshGenerator(Length, transform, Vector2.up);
        //ObstacleGenerator = new Autoroad_ObstacleGenerator();
    }

    void Update()
    {
        if(Car != null)
        {
            PlayerDistance = PathGenerator.CalculateDistanceAtPosition(Car.position);

            if(PathGenerator.Path.Count < Length - 1 || PlayerDistance - StartOffset > PathGenerator.DistanceDeleted + PathGenerator.Path[0].Offset)
            {
                PathGenerator.RefreshPath(PathSettings);
                MeshGenerator.RefreshMesh(PathGenerator.Path, Width, RoadsideWidth);
                //ObstacleGenerator.Grow(PlayerDistance, PathGenerator);
            }
        }
    }







}
