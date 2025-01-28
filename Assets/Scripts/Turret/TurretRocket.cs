using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRocket : TurretProjectile
{
    [Header("Rocket")] 
    [SerializeField] private float explosionSize = 3;
    
    protected override void OnTriggerEnter(Collider other) {
        if(spawnImmunity > 0) return;
        Explode();
    }

    private void Explode()
    {
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
