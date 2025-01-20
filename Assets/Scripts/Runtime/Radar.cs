using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float renderSpeed = 0.33f;
    private RadarRenderer radarRenderer;
    private float renderTimer;

    private void Start() {
        if (!radarRenderer) radarRenderer = FindObjectOfType<RadarRenderer>();
    }

    public void FixedUpdate() {
        renderTimer -= Time.fixedDeltaTime;
        if (renderTimer < 0 && radarRenderer) {
            radarRenderer.RenderAll(Scan(layerMask));
            renderTimer = renderSpeed;
        }
    }

    public List<GameObject> Scan(LayerMask layerMask) {
        Transform unwrapped = transform;
        Collider[] overlapResult = Physics.OverlapBox(unwrapped.position, unwrapped.lossyScale/2, Quaternion.identity, layerMask);

        if (overlapResult == null || overlapResult.Length == 0) {
            Debug.Log("Radar has not found any target");
            return new List<GameObject>();
        }

        List<GameObject> results = new List<GameObject>();
        foreach (var result in overlapResult) {
            results.Add(result.gameObject);   
        }

        return results;
    }
}
