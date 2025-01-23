using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRadarTarget
{
    public bool ShouldRenderAtRadar() { return false; }
    public Sprite GetRenderIcon();

    public float GetRenderSize() {
        return 1;
    }
}
