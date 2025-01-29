using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRocket : TurretProjectile
{
    [Header("Rocket")] 
    [SerializeField] protected float explosionSize = 3;
    [SerializeField] protected GameObject onHitEffect;
    [SerializeField] protected GameObject linkedTrailEffect;
    [SerializeField] protected float trailEffectDestroyOffset = 2;
    
    protected virtual void Explode() {
        Instantiate(onHitEffect, transform.position, Quaternion.identity);
        LeaveTrailBehind();
        Collider[] contacted = Physics.OverlapSphere(transform.position, explosionSize);
        foreach (var contact in contacted)
        {
            if (contact.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
                tank.DealDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }
    
    protected override void FixedUpdate() {
        Vector3 velocity = transform.forward * (speed * Time.fixedDeltaTime);
        bool didHit = Physics.Raycast(transform.position, velocity, out RaycastHit hit, velocity.sqrMagnitude*velocityToRayDistance, hitMask);
        if (didHit) {
            Explode();
        }
        else {
            transform.position += velocity;
        }
        
        LifetimeCheck(Time.fixedDeltaTime);
    }

    protected void LeaveTrailBehind() {
        linkedTrailEffect.GetComponent<ParticleSystem>().Stop();
        EffectAutoDestroy autoDestroy = linkedTrailEffect.gameObject.AddComponent<EffectAutoDestroy>();
        autoDestroy.time = trailEffectDestroyOffset;
        linkedTrailEffect.transform.SetParent(null);
    }
    
    
    protected override void OnLifetimeExpired() {
        Explode();
        Destroy(gameObject);
    }
}
