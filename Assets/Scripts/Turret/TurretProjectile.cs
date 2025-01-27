using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5;
    private int damage = 1;
    [SerializeField] private Rigidbody rigidbody;

    private void Launch(int damage, float speed, Vector3 direction) {
        if (!rigidbody) {
            Debug.LogWarning("Turret projectile has no rigidbody attached");
            Destroy(gameObject);
            return;
        }

        this.damage = damage;
        rigidbody.velocity = direction * speed;
    }
    
    public void Launch(int damage, float speed) {
        Launch(damage, speed, transform.forward);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
            tank.DealDamage(damage);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        lifetime -= Time.fixedDeltaTime;
        
        if (lifetime < 0) {
            Destroy(gameObject);
        }
    }
}
