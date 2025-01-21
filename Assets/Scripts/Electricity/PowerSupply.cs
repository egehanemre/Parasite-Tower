using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : MonoBehaviour, IEvent, Iinteractable
{
    [SerializeField] private GameObject connected;
    [SerializeField] private float possibility = 1;
    private IPowerDependent dependent;

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
    
    
    public bool CanHold(out float time) {
        time = 1;
        return true;
    }
}
