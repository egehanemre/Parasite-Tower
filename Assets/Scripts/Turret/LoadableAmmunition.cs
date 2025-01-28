using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadableAmmunition : MonoBehaviour
{
    [SerializeField] private SpecialAmmunition ammunition;
    public SpecialAmmunition GetAmmunition() {
        return ammunition;
    }
}

[Serializable]
public struct SpecialAmmunition
{
    public GameObject ammunitionPrefab;
    public int amount;
}