using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldLever : MonoBehaviour, Iinteractable
{
    public UnityEvent<string> onLeverPulled = new UnityEvent<string>();
    [SerializeField] private string side;
    [SerializeField] private float leverRotation;
    
    public void Interact() {
        onLeverPulled?.Invoke(side);
    }

    public bool MatchesSide(string targetSide) {
        return side == targetSide;
    }

    public void UpdateVisualizer(bool isActive) {
        float newEulerZ = 0;
        if (isActive) newEulerZ = leverRotation;
        else newEulerZ -= leverRotation;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newEulerZ);
    }
}
