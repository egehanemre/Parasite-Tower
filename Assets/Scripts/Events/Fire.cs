using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IEvent, Iinteractable, IRadarTarget
{
    [Header("Stages")]
    private int currentStage;
    [SerializeField] private GameObject[] stages;
    [SerializeField] private float damageTickTime = 1;
    [SerializeField] private float damagePerStage = 2;

    [Header("Settings")] 
    public float tickPossibility = 1;
    private HealthManager healthRef;
    [SerializeField] private float extinguishingTime = 0.33f; 
    [SerializeField] private Vector3 radarOffset = Vector3.zero;

    private void Start() {
        if (!healthRef) healthRef = FindObjectOfType<HealthManager>();
        StartCoroutine(FireDamageLoop());
        ChangeStage(0);
    }

    public void ClearRender() {
        foreach (var stage in stages) { stage.SetActive(false); }
    }

    public void Render(int stage) {
        for (int i = 0; i < stage && i < stages.Length; i++) { stages[i].SetActive(true); }
    }

    public void ChangeStage(int change) {
        currentStage = Mathf.Clamp(change+currentStage, 0, stages.Length);
        ClearRender();
        Render(currentStage);
    }

    private IEnumerator FireDamageLoop() {
        while (healthRef) {
            yield return new WaitForSeconds(damageTickTime);
            if (currentStage > 0) {
                healthRef.OnDamageTaken(damagePerStage*currentStage, "inside");
            }
        }
    }

    public void Tick() {
        ChangeStage(1);
    }

    public float GetWeight() {
        return tickPossibility;
    }

    public bool CanHold(out float time) {
        time = extinguishingTime;
        return true;
    }

    public bool ShouldRenderAtRadar() {
        return currentStage > 0;
    }

    public Color GetRadarColor() {
        return Color.Lerp(Color.red, Color.black, currentStage*0.25f);
    }

    public Vector3 RadarRenderOffset() {
        return radarOffset;
    }

}
