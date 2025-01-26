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
    [SerializeField] private Vector3 radarOffset = Vector3.zero;
    private TowerHealthManager healthRef;

    private void Start()
    {
        healthRef = FindObjectOfType<TowerHealthManager>();
        if (!healthRef)
        {
            Debug.LogWarning("HealthManager not found in the scene.", gameObject);
        }

        StartCoroutine(FireDamageLoop());
        ChangeStage(0);
        ResetGrowthTimer();
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

    public bool ShouldRenderAtRadar()
    {
        return currentStage > 0;
    }
    public void Interact()
    {
        if (currentStage > 0)
        {
            ChangeStage(-1);
            Debug.Log("Fire interacted with and reduced in stage.");
        }
    }
    public void InteractOnHolding(Transform pov, float holdingTime)
    {
        if (holdingTime >= extinguishingTime)
        {
            if (currentStage > 0)
            {
                ChangeStage(-1); 
                Debug.Log("Fire extinguished while holding.");
            }
        }
    }
    public void SetPhysicsMode(bool isActive)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        if (rb)
        {
            rb.isKinematic = !isActive;
        }

        if (col)
        {
            col.enabled = isActive;
        }
    }
    public bool CanHold(out float time)
    {
        time = extinguishingTime; 
        return true;
    }

    public bool CanGrab()
    {
        return false; 
    }
}
