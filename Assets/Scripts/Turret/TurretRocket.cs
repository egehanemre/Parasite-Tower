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
    [SerializeField] protected LayerMask hitMask;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float velocityToRayDistance = 2;

    protected override void Launch(int damage, float speed, Vector3 direction) {
        this.speed = speed;
        this.damage += damage;
    }

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
        base.FixedUpdate();

        Vector3 velocity = transform.forward * (speed * Time.fixedDeltaTime);
        bool didHit = Physics.Raycast(transform.position, velocity, out RaycastHit hit, velocity.sqrMagnitude*velocityToRayDistance, hitMask);
        if (didHit) {
            Explode();
        }
        else {
            transform.position += velocity;
        }
    }

    protected void LeaveTrailBehind() {
        linkedTrailEffect.GetComponent<ParticleSystem>().Stop();
        EffectAutoDestroy autoDestroy = linkedTrailEffect.gameObject.AddComponent<EffectAutoDestroy>();
        autoDestroy.time = trailEffectDestroyOffset;
        linkedTrailEffect.transform.SetParent(null);
    }
    
    
    protected override void OnLifetimeExpired()
    {
        Explode();
        Destroy(gameObject);
    }
}
