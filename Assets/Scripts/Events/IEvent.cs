using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    public void Tick();
    public float GetWeight();
    public Color GetRadarColor() { return Color.red; }
    public bool ShouldRenderAtRadar() { return false; }
}
