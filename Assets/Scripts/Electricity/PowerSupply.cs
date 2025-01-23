using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : MonoBehaviour, IEvent, Iinteractable
{
    [SerializeField] private GameObject connected;
    [SerializeField] private float possibility = 1;
    private IPowerDependent dependent;
    [SerializeField] private float fixingCooldown = 0.33f;
    [SerializeField] private Vector3 radarOffset = Vector3.zero;

    private void Start() {
        if (!connected) {
            Debug.Log("There is no object this power supply connected", gameObject);
            return;
        }
        dependent = connected.GetComponent<IPowerDependent>();
    }

    public void Tick() {
        if (dependent == null) {
            Debug.Log("Power supply connection went wrong");
            return;
        }
        
        dependent.SetPowerState(false);
    }

    public void Fix() {
        dependent.SetPowerState(true);
    }
    
    public float GetWeight() {
        return possibility;
    }

    public bool IsActive()
    {
        return !dependent.GetPowerState();
    }

    public bool CanHold(out float time) {
        time = fixingCooldown;
        return true;
    }

    public void InteractOnHolding(Transform pov, float holdingTime)
    {
        Fix();
    }
}
