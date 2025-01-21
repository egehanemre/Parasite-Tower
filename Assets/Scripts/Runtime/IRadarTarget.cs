using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRadarTarget
{
    public Color GetRadarColor() { return Color.red; }
    public bool ShouldRenderAtRadar() { return false; }
    public Vector3 RadarRenderOffset() { return new Vector3(0, 0, 0); }
}
