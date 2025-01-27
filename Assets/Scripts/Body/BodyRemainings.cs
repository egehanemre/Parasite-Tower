using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRemainings : MonoBehaviour
{
    [SerializeField] private GameObject skullPrefab;

    public void Collect() {
        GameObject spawnedSkull = Instantiate(skullPrefab);
        spawnedSkull.transform.position = transform.position;
        Destroy(gameObject);
    }
}