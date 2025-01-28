using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTurretCable : MonoBehaviour
{
    [SerializeField] private Turret linkedTurret;
    public void Fix() {
        linkedTurret.RecoverEnergy();
        gameObject.SetActive(false);
    }
}