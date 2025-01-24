using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRemainings : MonoBehaviour, Iinteractable
{
    [SerializeField] private float collectingTime = 5;
    [SerializeField] private GameObject skullPrefab;

    public void Collect()
    {
        GameObject spawnedSkull = Instantiate(skullPrefab);
        spawnedSkull.transform.position = transform.position;
        Destroy(gameObject);
    }

    public bool CanHold(out float time) {
        time = collectingTime;
        return true;
    }

    public void InteractOnHolding(Transform pov, float holdingTime)
    {
        Collect();
    }
}
