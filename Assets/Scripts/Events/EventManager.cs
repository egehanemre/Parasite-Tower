using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Event Cache")]
    private List<IEvent> eventSlots = new List<IEvent>();
    private readonly List<EventWeight> calculatedEventWeights = new List<EventWeight>();
    private float totalWeight = 0;

    [Header("Event Throwing")]
    [SerializeField] private float throwInterval = 5f;
    [SerializeField] private float intervalRandomization = 2f;
    [SerializeField] private float throwRate = 0.85f;
    [SerializeField] private int maxParallelEventAmount = 3;
    [SerializeField] private float activeEventsToThrowRate = -0.1f;
    private float throwTimer = 0;

    private void Start() {
        GameObject[] eventObjects = GameObject.FindGameObjectsWithTag("Event");
        CacheEvents(eventObjects);
        CalculateWeights();
    }

    private void FixedUpdate()
    {
        throwTimer -= Time.fixedDeltaTime;
        if (throwTimer <= 0)
        {
            PerformThrowingTick();
        }
    }

    private void PerformThrowingTick()
    {
        throwTimer = UnityEngine.Random.Range(throwInterval, throwInterval * intervalRandomization);

        int activeEventAmount = GetActiveEventAmount();
        
        if (UnityEngine.Random.value < (throwRate + activeEventAmount*activeEventsToThrowRate) && activeEventAmount < maxParallelEventAmount) {
            CalculateWeights();
            ThrowRandomEvent();
        }
    }

    public void CacheEvents(GameObject[] objects)
    {
        eventSlots.Clear();
        foreach (var eventPiece in objects)
        {
            var eventComponent = eventPiece.GetComponent<IEvent>();

            if (eventComponent != null)
            {
                if(eventComponent.IsActive()) continue;
                eventSlots.Add(eventComponent);
            }
            else
            {
                Debug.LogWarning($"GameObject {eventPiece.name} does not implement IEvent");
            }
        }
    }

    public void ThrowRandomEvent()
    {
        Shuffle(calculatedEventWeights);
        float randomWeight = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var eventPiece in calculatedEventWeights)
        {
            currentWeight += eventPiece.Weight;
            if (currentWeight >= randomWeight)
            {
                eventPiece.EventRef.Tick();
                var gm = eventPiece.EventRef as MonoBehaviour;
                Debug.Log("Event Thrown: " + gm.gameObject.name);
                break;
            }
        }
    }

    public void CalculateWeights()
    {
        if (eventSlots == null || eventSlots.Count == 0)
        {
            Debug.LogWarning("No events found to calculate");
            return;
        }

        calculatedEventWeights.Clear();
        totalWeight = 0;

        foreach (var eventSlot in eventSlots)
        {
            float weight = eventSlot.GetWeight();
            calculatedEventWeights.Add(new EventWeight { Weight = weight, EventRef = eventSlot });
            totalWeight += weight;
        }

        if (totalWeight <= 0)
        {
            Debug.LogError("Total weight of all events is zero or negative");
        }
    }
    
    public int GetActiveEventAmount() {
        int activeEventAmount = 0;
        foreach (var eventInstance in eventSlots) {
            if (eventInstance.IsActive()) activeEventAmount++;
        }

        return activeEventAmount;
    }

    public void Shuffle(List<EventWeight> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

public struct EventWeight
{
    public float Weight;
    public IEvent EventRef;
}
