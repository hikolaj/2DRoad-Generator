using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Autoroad_PathGenerator
{
    public int PointsCreated { get; private set; }
    public int PointsDeleted { get; private set; }
    public float DistanceCreated { get; private set; }
    public float DistanceDeleted { get; private set; }

    public List<Autoroad_Point> Path;

    private float _size;
    private float _rotation;
    private int _divisions;
    private int _newPointCounter;

    private Transform _roadTransform;
    private int _maxLength;

    private static float _vectorCalculatorOffset = 0.01f;
    private static float _minRotation = 45;
    private static float _rotationDivider = 45;

    public Autoroad_PathGenerator(int length, Transform transform)
    {
        _roadTransform = transform;
        _maxLength = length;
        _newPointCounter = 0;

        PointsCreated = 0;
        PointsDeleted = 0;
        DistanceCreated = 0;
        DistanceDeleted = 0;

        Path = new List<Autoroad_Point>();
        Path.Add(new Autoroad_Point(15, 0));
    }

    public void RefreshPath(Autoroad_PathGeneratorSettings settings)
    {            
        if (Path.Count < _maxLength)
        {
            do
            {
                if (_newPointCounter >= _divisions)
                {
                    CreateNewPattern(settings);
                }
                
                AddNewPoint();
            }
            while (Path.Count < _maxLength);

            if (Path.Count >= _maxLength)
            {
                DeleteFirstPoint();
            }
        }
    }

    public float CalculateDistanceAtPosition(Vector2 o)// Calculates distance to nearest point N on a segment line (perp. to the direction);
    {
        if (Path.Count == 0)
        {
            return 0;
        }

        float distance = 0;
        Vector2 dir = _roadTransform.up;
        Autoroad_Point point = Path[0];

        Vector2 a = _roadTransform.position;
        Vector2 b = a;

        for (int i = 0; i < Path.Count; i++)
        {
            point = Path[i];
            dir = dir.Rotate(point.Rotation);
            b = a + (dir * point.Offset);

            if (Vector2.Dot((o - b).normalized, dir) < 0)
            {
                Vector2 foot = Autoroad_Extensions.FootIntersectionPoint(a, b, o);// N
                distance += Vector2.Distance(a, foot);
                break;
            }
            else
            {
                a = b;
                distance += point.Offset;
            }
        }

        return distance + DistanceDeleted;
    }

    public Vector2 CalculatePositionAtDistance(float distance)
    {
        distance -= DistanceDeleted;
        float distanceAt = 0;
        Vector2 position = _roadTransform.position;
        Vector2 direction = _roadTransform.up;

        for(int i = 0; i < Path.Count; i++)
        {
            Autoroad_Point point = Path[i];
            if ( distance > distanceAt + point.Offset)// full offset
            {
                distanceAt += point.Offset;
                direction = direction.Rotate(point.Rotation);
                position += direction * point.Offset;
            }
            else if(distance > distanceAt)// Found
            {
                direction = direction.Rotate(point.Rotation);

                return direction * (distance - distanceAt);
            }
        }

        return Vector2.zero;
    }

    public Vector2 CalculateForwardAtDistance(float distance)
    {
        if (DistanceDeleted < distance)
        {
            float offset = DistanceCreated <= distance ? _vectorCalculatorOffset : -_vectorCalculatorOffset;

            if (distance + offset > DistanceCreated)
            {
                Vector2 pos = CalculatePositionAtDistance(distance);
                Vector2 posOffseted = CalculatePositionAtDistance(distance + offset);
                pos = (posOffseted - pos).normalized * Mathf.Sign(offset);
                return pos;
            }
        }

        return Vector2.zero;
    }

    private void CreateNewPattern(Autoroad_PathGeneratorSettings settings)
    {
        _newPointCounter = 0;

        if (settings.Randomize)
        {
            _size = Random.Range(settings.MinSize, settings.MaxSize);
            _rotation = Random.Range(settings.MinRot, settings.MaxRot);
        }
        else
        {
            _size = settings.Size;
            _rotation = settings.Rotation;
        }

        _divisions = _rotation == 0 ? 1 : (int)((Mathf.Pow(_size, 0.5f) * ((Mathf.Abs(_rotation) + _minRotation) / _rotationDivider)) + 1);
    }

    private void AddNewPoint()
    {
        Autoroad_Point nextPoint = new Autoroad_Point(_size / _divisions, _rotation / _divisions);
        _newPointCounter++;
        PointsCreated += 1;
        DistanceCreated += nextPoint.Offset;
        Path.Add(nextPoint);
    }

    private void DeleteFirstPoint()
    {
        Autoroad_Point deletedPoint = Path[0];
        _roadTransform.Rotate(Vector3.back, deletedPoint.Rotation);
        _roadTransform.position += _roadTransform.up * deletedPoint.Offset;
        DistanceDeleted += deletedPoint.Offset;
        PointsDeleted += 1;
        Path.RemoveAt(0);
    }

}