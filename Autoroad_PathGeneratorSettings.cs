using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoroad_PathGeneratorCalculatedSettings
{
    public float Rotation;
    public float Size;
    public int Divisions;
}

[CreateAssetMenu(fileName = "RoadFragment", menuName = "Autoroad/Fragment")]
public class Autoroad_PathGeneratorSettings : ScriptableObject
{
    public float Rotation;
    public float Size;
    public int Divisions;
    public bool Randomize;
    public float MinRot;
    public float MaxRot;
    public float MinSize;
    public float MaxSize;
}
