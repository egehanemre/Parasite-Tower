using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IEvent, Iinteractable
{
    [Header("Stages")]
    public int currentStage;
    public GameObject[] stages;

    [Header("Settings")] public float tickPossibility = 1;

    private void Start() {
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

    public void Tick() {
        ChangeStage(1);
    }

    public float GetWeight()
    {
        return tickPossibility;
    }

    public bool CanHold(out float time)
    {
        time = 1;
        return true;
    }
}
