using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRocket : TurretProjectile
{
    [Header("Rocket")] 
    [SerializeField] protected float explosionSize = 3;
    [SerializeField] protected GameObject onHitEffect;
    
    protected override void OnTriggerEnter(Collider other) {
        if(spawnImmunity > 0) return;
        Explode();
    }

    protected virtual void Explode() {
        Instantiate(onHitEffect, transform.position, Quaternion.identity);
        Collider[] contacted = Physics.OverlapSphere(transform.position, explosionSize);
        foreach (var contact in contacted)
        {
            if (contact.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
                tank.DealDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }
}
