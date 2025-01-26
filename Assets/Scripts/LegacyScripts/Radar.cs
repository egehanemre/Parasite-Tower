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
        if (!radarRenderer) radarRenderer = FindObjectOfType<RadarRenderer>(true);
    }

    public void FixedUpdate() {
        renderTimer -= Time.fixedDeltaTime;
        if (renderTimer < 0 && radarRenderer) {
            radarRenderer.RenderAll(Scan(layerMask));
            renderTimer = renderSpeed;
        }
    }

    public List<RadarEntity> Scan(LayerMask layerMask) {
        Transform unwrapped = transform;
        Collider[] overlapResult = Physics.OverlapBox(unwrapped.position, unwrapped.lossyScale/2, Quaternion.identity, layerMask);

        if (overlapResult == null || overlapResult.Length == 0) {
            Debug.Log("Radar has not found any target");
            return null;
        }

        List<RadarEntity> results = new List<RadarEntity>();
        foreach (var result in overlapResult) {
            RadarEntity newEntity = new RadarEntity();
            newEntity.gameObject = result.gameObject;
            newEntity.relativePosition = transform.position - result.gameObject.transform.position;
            newEntity.scanZoneScale = transform.localScale;
            results.Add(newEntity);   
        }

        return results;
    }
}

public struct RadarEntity
{
    public Vector3 scanZoneScale;
    public Vector3 relativePosition;
    public GameObject gameObject;
}
