using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarRenderer : MonoBehaviour
{
    private List<GameObject> renderedTargets = new List<GameObject>();
    [SerializeField] private GameObject radarTargetPrefab;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float scaleToRender = 1;
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void RenderAll(List<RadarEntity> targets) {
        ClearRender();
        gameObject.SetActive(true);

        foreach (var target in targets) {
            if(!target.gameObject) continue;

            if (target.gameObject.TryGetComponent<IRadarTarget>(out IRadarTarget radarTarget)) {
                if(!radarTarget.ShouldRenderAtRadar()) continue;
                Sprite icon = radarTarget.GetRenderIcon();
                Render(icon, target.relativePosition, target.scanZoneScale, radarTarget.GetRenderSize());
            }
        }
    }

    public void Render(Sprite icon, Vector3 position, Vector3 scale, float iconSize) {
        if (!radarTargetPrefab) {
            Debug.Log("No prefab for radar target found.");
            return;
        }

        GameObject spawnedRadarTarget = Instantiate(radarTargetPrefab, transform);
        renderedTargets.Add(spawnedRadarTarget);
        spawnedRadarTarget.transform.localPosition = new Vector3(-rectTransform.sizeDelta.x/2, -rectTransform.sizeDelta.y/2);
        spawnedRadarTarget.transform.localPosition += new Vector3(position.x*scale.x, position.y*scale.y)*scaleToRender;
        
        //spawnedRadarTarget.transform.localPosition = new Vector3(-xInBounds, -yInBounds, 0) + offset;
        if (spawnedRadarTarget.TryGetComponent<Image>(out Image image)) {
            image.sprite = icon;
            spawnedRadarTarget.transform.localScale *= iconSize;
        }
    }

    public void ClearRender() {
        if (renderedTargets == null || renderedTargets.Count == 0) {
            Debug.Log("No renders found in Radar Renderer");
            return;
        }

        foreach (var rendered in renderedTargets) {
            Destroy(rendered);
        }
        renderedTargets.Clear();
    }
}
