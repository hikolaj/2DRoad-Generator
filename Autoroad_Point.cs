using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Autoroad_Point
{
    public float Offset;
    public float Rotation;
    
    public Vector2 WorldPosition { get; private set; }
    public Vector2 WorldDirecion { get; private set; }

    public Autoroad_Point(float offset, float rotation)
    {
        Offset = offset;
        Rotation = rotation;
        WorldPosition = Vector2.zero;
        WorldDirecion = Vector2.up;
    }

    public Autoroad_Point(float offset, float rotation, Autoroad_Point pointBefore)
    {
        Offset = offset;
        Rotation = rotation;
        WorldDirecion = pointBefore.WorldDirecion;
        WorldDirecion.Rotate(Rotation);
        WorldPosition = pointBefore.WorldPosition + WorldDirecion * Offset;
    }

    public void CalculateWorldPosAndDir(Autoroad_Point pointBefore)
    {
        WorldDirecion = pointBefore.WorldDirecion;
        WorldDirecion.Rotate(Rotation);
        WorldPosition = pointBefore.WorldPosition + WorldDirecion * Offset;
    }
}
