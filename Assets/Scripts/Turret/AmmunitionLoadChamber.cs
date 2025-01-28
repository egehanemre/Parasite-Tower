using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionLoadChamber : MonoBehaviour
{
    [SerializeField] private Turret linkedTurret;

    public void FeedChamber(SpecialAmmunition ammunition) {
        linkedTurret.LoadAmmo(ammunition);
    }
}
