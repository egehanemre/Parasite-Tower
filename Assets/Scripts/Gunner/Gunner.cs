using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    [SerializeField] private GunnerPosition position;
    [SerializeField] private Transform reachZone;
    [SerializeField] private LayerMask targetsMask;
    [SerializeField] private int lifetime = 10;
    [SerializeField] private float shootCooldown = 1;

    private void Start()
    {
        StartCoroutine(ShootLoop());
    }

    private IEnumerator ShootLoop()
    {
        position.beingUsed = true;
        for (int i = 0; i < lifetime; i++) {
            yield return new WaitForSeconds(shootCooldown);
            if(position && position.GetPowerState()) HitRandomTarget();
        }

        position.beingUsed = false;
        Destroy(gameObject);
    }

    private void HitRandomTarget() {
        Collider[] allTargets = GetAllTargets();

        if (allTargets == null || allTargets.Length == 0) {
            Debug.Log("No targets available for gunner");
            return;
        }

        GameObject firstTarget = allTargets[0].gameObject;
        if (firstTarget.TryGetComponent<Tank>(out Tank tank)) {
            tank.HitTank();
        }
    }
    
    private Collider[] GetAllTargets() {
        if (!reachZone) {
            Debug.Log("No reach zone for gunner");
            return null;
        }
        Collider[] overlapResult = Physics.OverlapBox(reachZone.position, reachZone.lossyScale/2, Quaternion.identity, targetsMask);
        return overlapResult;
    }
}
