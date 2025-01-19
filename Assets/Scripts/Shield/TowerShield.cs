using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShield : MonoBehaviour
{
    [SerializeField] private string side;
    [SerializeField] private bool activated;
    [SerializeField] private GameObject visualizer;

    public void Turn(bool on) {
        activated = on;
        if(visualizer)visualizer.SetActive(on);
    }

    public bool DoesProtect(string targetSide) {
        return targetSide == side && activated;
    }

    public bool MatchesSide(string targetSide) {
        return targetSide == side;
    }
}
