using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTurretCable : MonoBehaviour, IinteractionInformationTaker
{
    [SerializeField] private AudioSource fixingSound;
    [SerializeField] private Turret linkedTurret;
    public void Fix() {
        linkedTurret.RecoverEnergy();
        gameObject.SetActive(false);
    }
    
    
    public void HasBeenHoldingSince(float time, bool holdingNow) {
        fixingSound.mute = !holdingNow;
    }
}