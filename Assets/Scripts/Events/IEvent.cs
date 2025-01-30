using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    public void Tick();
    public float GetWeight();
    public bool IsActive();
    public EventData GetData();
}

public struct EventData
{
    public int floor;
    public EventType eventType;
}

public enum EventType
{
    Fire,
    Electric
}
