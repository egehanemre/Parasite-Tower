using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStock : MonoBehaviour
{
    [SerializeField] private GameObject rocketAmmunitionPrefab;

    public void Spawn() {
        Instantiate(rocketAmmunitionPrefab, transform.position, transform.rotation);
    }
}
