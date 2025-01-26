using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour, IEvent
{
    [Header("Stages")]
    private int currentStage;
    [SerializeField] private GameObject[] stages;
    [SerializeField] private float damageTickTime = 1f;
    [SerializeField] private float damagePerStage = 2f;
    [SerializeField] private float stageGrowthTime = 10f;
    [SerializeField] private float stageGrowthRandomization = 2f;
    private float calculatedStageGrowthTime;

    [Header("Settings")]
    [SerializeField] private float tickPossibility = 1f;
    [SerializeField] private float extinguishingTime = 0.33f;
    private TowerHealthManager healthRef;

    [Header("Extinguishing")] 
    private float extinguishingProgress = 0;

    private void Start()
    {
        healthRef = FindObjectOfType<TowerHealthManager>();
        if (!healthRef)
        {
            Debug.LogWarning("HealthManager not found in the scene.", gameObject);
        }

        StartCoroutine(FireDamageLoop());
        ResetGrowthTimer();
        Render(currentStage);
    }

    private void FixedUpdate()
    {
        calculatedStageGrowthTime -= Time.fixedDeltaTime;

        if (calculatedStageGrowthTime <= 0f)
        {
            ResetGrowthTimer();

            if (currentStage > 0)
            {
                ChangeStage(1);
            }
        }
    }

    private void ResetGrowthTimer()
    {
        calculatedStageGrowthTime = Random.Range(
            stageGrowthTime - stageGrowthRandomization,
            stageGrowthTime + stageGrowthRandomization
        );
    }

    private void ClearRender()
    {
        foreach (var stage in stages)
        {
            stage.SetActive(false);
        }
    }

    private void Render(int stage)
    {
        ClearRender();
        for (int i = 0; i < stage && i < stages.Length; i++)
        {
            stages[i].SetActive(true);
        }
    }

    public void ChangeStage(int change)
    {
        int newStage = Mathf.Clamp(currentStage + change, 0, stages.Length);
        if (newStage != currentStage)
        {
            currentStage = newStage;
            ClearRender();
            Render(currentStage);
            Debug.Log($"Fire stage changed to: {currentStage}");
        }
    }

    private IEnumerator FireDamageLoop()
    {
        while (healthRef)
        {
            yield return new WaitForSeconds(damageTickTime);

            if (currentStage > 0 && healthRef != null)
            {
                healthRef.TakeDamage(damagePerStage * currentStage);
            }
        }
    }
    public void Tick()
    {
        ChangeStage(1);
    }

    public float GetWeight()
    {
        return tickPossibility;
    }

    public bool IsActive()
    {
        return currentStage > 0;
    }

    public void Extinguish(float progress) {
        extinguishingProgress += progress;
        if (extinguishingProgress > 1) {
            extinguishingProgress--;
            ChangeStage(-1);
        }
    }
}
