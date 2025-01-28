using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour, IEvent
{
    [SerializeField] private CutTurretCable cutCable;
    [SerializeField] private Turret linkedTurret;
    [SerializeField] private float cutProbability = 0.25f;

    public void Tick() {
        linkedTurret.CutEnergy();
        cutCable.gameObject.SetActive(true);
    }

    public float GetWeight() {
        return cutProbability;
    }

    public bool IsActive() {
        return cutCable.gameObject.activeSelf;
    }
}
