using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Autoroad_MeshGenerator
{
    public int MaxPoints { get; private set; }
    public Mesh RoadMesh { get; private set; }
    public Mesh RoadsideMesh { get; private set; }

    private Vector2 _lastDirection;
    private Vector3[] _verts;
    private Vector3[] _vertsRoadsides;
    private Vector2[] _uvs;
    private Vector3[] _normals;
    private int[] _tris;
    private int _vertIndex = 0;

    private MeshFilter _road_meshFilter;
    private MeshFilter _roadside_meshFilter;

    private static float _uvOffsetScale = 0.1f;

    public Autoroad_MeshGenerator(int maxPoints, Transform transform, Vector2 direction)
    {
        MaxPoints = maxPoints;
        _lastDirection = direction;
        _verts = new Vector3[maxPoints * 2];
        _vertsRoadsides = new Vector3[maxPoints * 2];
        _uvs = new Vector2[maxPoints * 2];
        _normals = new Vector3[maxPoints * 2];
        _tris = new int[(maxPoints - 1) * 6];

        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < maxPoints; i++)
        {
            _normals[_vertIndex] = Vector3.back;
            _normals[_vertIndex + 1] = Vector3.back;

            if (i < maxPoints - 1)
            {
                _tris[triIndex] = vertIndex;
                _tris[triIndex + 1] = vertIndex + 2;
                _tris[triIndex + 2] = vertIndex + 1;
                _tris[triIndex + 3] = vertIndex + 1;
                _tris[triIndex + 4] = vertIndex + 2;
                _tris[triIndex + 5] = vertIndex + 3;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        RoadMesh = new Mesh();
        RoadMesh.name = "Road_Mesh";
        RoadMesh.SetVertices(_verts);
        RoadMesh.SetUVs(0, _uvs);
        RoadMesh.SetNormals(_normals);
        RoadMesh.SetTriangles(_tris, 0);

        RoadsideMesh = new Mesh();
        RoadsideMesh.name = "Roadside_Mesh";
        RoadsideMesh.SetVertices(_vertsRoadsides);
        RoadsideMesh.SetUVs(0, _uvs);
        RoadsideMesh.SetNormals(_normals);
        RoadsideMesh.SetTriangles(_tris, 0);
        
        _road_meshFilter = transform.GetComponent<MeshFilter>();
        _road_meshFilter.mesh = RoadMesh;

        Transform roadside = transform.GetChild(0);
        _roadside_meshFilter = roadside.GetComponent<MeshFilter>();
        _roadside_meshFilter.mesh = RoadsideMesh;
    }
    
    public void RefreshMesh(List<Autoroad_Point> points, float width, float roadsideWidth)
    {
        int pointAmount = points.Count;
        Vector2 lastPoint = Vector2.zero;
        Vector2 startDirection = Vector2.up;
        Vector2 right = new Vector3(-startDirection.y, startDirection.x);
        Vector2 offset = right * width / 2;
        Vector2 offsetRoadside = right * roadsideWidth / 2;

        _vertIndex = 2;
        _lastDirection = startDirection;
        _verts[0] = offset;
        _verts[1] = -offset;
        _vertsRoadsides[0] = offsetRoadside;
        _vertsRoadsides[1] = -offsetRoadside;
        _uvs[0] = new Vector2(0, 0);
        _uvs[1] = new Vector2(1, 0);

        for (int i = 0; i < MaxPoints - 1; i++)
        {
            if (i < pointAmount)
            {
                _lastDirection = Quaternion.AngleAxis(points[i].Rotation, Vector3.back) * _lastDirection;
                lastPoint += _lastDirection * points[i].Offset;

                right = new Vector3(-_lastDirection.y, _lastDirection.x);
                offset = right * width / 2;
                offsetRoadside = right * roadsideWidth / 2;

                _verts[_vertIndex] = lastPoint + offset;
                _verts[_vertIndex + 1] = lastPoint - offset;
                _vertsRoadsides[_vertIndex] = lastPoint + offsetRoadside;
                _vertsRoadsides[_vertIndex + 1] = lastPoint - offsetRoadside;

                _uvs[_vertIndex] = new Vector2(0, Vector2.Distance(_verts[_vertIndex], _verts[_vertIndex - 2]) * _uvOffsetScale + _uvs[_vertIndex - 2].y);
                _uvs[_vertIndex + 1] = new Vector2(1, Vector2.Distance(_verts[_vertIndex + 1], _verts[_vertIndex - 1]) * _uvOffsetScale + _uvs[_vertIndex - 1].y);
            }
            else
            {
                _verts[_vertIndex] = _verts[pointAmount * 2];
                _verts[_vertIndex + 1] = _verts[pointAmount * 2 + 1];
                _vertsRoadsides[_vertIndex] = _verts[pointAmount * 2];
                _vertsRoadsides[_vertIndex + 1] = _verts[pointAmount * 2 + 1];
                _uvs[_vertIndex] = new Vector2(0, 0);
                _uvs[_vertIndex + 1] = new Vector2(1, 0);
            }

            _vertIndex += 2;
        }

        RoadMesh.SetVertices(_verts);
        RoadMesh.SetUVs(0, _uvs);
        RoadMesh.RecalculateBounds();

        RoadsideMesh.SetVertices(_vertsRoadsides);
        RoadsideMesh.SetUVs(0, _uvs);
        RoadsideMesh.RecalculateBounds();
    }
    
}
