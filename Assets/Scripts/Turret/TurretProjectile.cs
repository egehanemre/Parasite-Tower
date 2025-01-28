using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] protected float lifetime = 5;
    [SerializeField] protected float spawnImmunity = 0.15f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float projectileSpeed = 1;
    protected Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Launch(int damage, float speed, Vector3 direction) {
        if (!rigidbody) {
            Debug.LogWarning("Turret projectile has no rigidbody attached");
            Destroy(gameObject);
            return;
        }

        this.damage += damage;
        rigidbody.velocity = direction * (speed * projectileSpeed);
    }
    
    public void Launch(int damage, float speed) {
        Launch(damage, speed, transform.forward);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if(spawnImmunity > 0) return;
        
        if (other.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
            tank.DealDamage(damage);
        }
        
        Destroy(gameObject);
    }

    private void FixedUpdate() {
        lifetime -= Time.fixedDeltaTime;
        if (spawnImmunity > 0) spawnImmunity -= Time.fixedDeltaTime;
        
        if (lifetime < 0) {
            Destroy(gameObject);
        }
    }
}
