using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarRenderer : MonoBehaviour
{
    private List<GameObject> renderedTargets = new List<GameObject>();
    [SerializeField] private GameObject radarTargetPrefab;
    [SerializeField] private float positionMultiplier = 1;
    [SerializeField] private Vector3 positionOffset;
    public void RenderAll(List<GameObject> targets) {
        ClearRender();
        gameObject.SetActive(true);

        foreach (var target in targets) {
            if(!target) continue;

            if (target.TryGetComponent<IRadarTarget>(out IRadarTarget radarTarget)) {
                if(!radarTarget.ShouldRenderAtRadar()) continue;
                Color color = radarTarget.GetRadarColor();
                Render(color, target.transform.position + radarTarget.RadarRenderOffset());
            }
        }
    }

    public void Render(Color color, Vector3 position) {
        if (!radarTargetPrefab) {
            Debug.Log("No prefab for radar target found.");
            return;
        }

        GameObject spawnedRadarTarget = Instantiate(radarTargetPrefab, transform);
        renderedTargets.Add(spawnedRadarTarget);
        spawnedRadarTarget.transform.localPosition = new Vector3(position.x*positionMultiplier, position.y*positionMultiplier, 0) + positionOffset;
        if (spawnedRadarTarget.TryGetComponent<Image>(out Image image)) {
            image.color = color;
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
