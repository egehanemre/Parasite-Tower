using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] protected float lifetime = 5;
    [SerializeField] protected float spawnImmunity = 0.15f;
    [SerializeField] public int damage = 1;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float velocityToRayDistance = 2;
    [SerializeField] protected LayerMask hitMask;
    [SerializeField] protected AudioClip audioOnLaunch;
    [SerializeField] protected AudioClip audioOnContact;
    [SerializeField] protected float audioPower = 0.6f;

    protected virtual void Launch(int damage, float speed, Vector3 direction) {
        this.speed = speed;
        this.damage += damage;
        if(audioOnLaunch && SoundEffectsManager.isGunnerMode)AudioSource.PlayClipAtPoint(audioOnLaunch, transform.position, audioPower);
    }
    
    public void Launch(int damage, float speed) {
        Launch(damage, speed, transform.forward);
    }
    
    protected virtual void FixedUpdate() {  
        Vector3 velocity = transform.forward * (speed * Time.fixedDeltaTime);
        bool didHit = Physics.Raycast(transform.position, velocity, out RaycastHit hit, velocity.sqrMagnitude*velocityToRayDistance, hitMask);
        if (didHit) {
            if (hit.rigidbody && hit.rigidbody.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
                if(audioOnContact)AudioSource.PlayClipAtPoint(audioOnContact, transform.position, audioPower);
                tank.DealDamage(damage, hit.point);
            }
            
            Destroy(gameObject);
        }
        else {
            transform.position += velocity;
        }
        
        LifetimeCheck(Time.fixedDeltaTime);
    }

    protected virtual void LifetimeCheck(float deltaTime) {
        lifetime -= Time.fixedDeltaTime;
        if (spawnImmunity > 0) spawnImmunity -= Time.fixedDeltaTime;
        
        if (lifetime < 0) {
            OnLifetimeExpired();
        }
    }

    protected virtual void OnLifetimeExpired()
    {
        Destroy(gameObject);
    }
}
