using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tank : MonoBehaviour
{
    [SerializeField] private float thrustPower = 1f;
    [SerializeField] private float thrustRandomization = 1.25f;
    [SerializeField] private float thrustRandomizationOffset = 0.35f;
    [SerializeField] private float damage = 5;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 10;
    [SerializeField] private Rigidbody rigidbody;
    private float fixedYaw;
    private float thrustTimer = 0;
    private float attackTimer = 0;
    private Transform target;
    private float calculatedThrust = 0.55f;
    private float hitCheckTimer = 0;

    public void Setup(Transform targetInstance) {
        fixedYaw = transform.rotation.y;
        target = targetInstance;
    }

    public void FixedUpdate() {
        
        if (IsInRange())
        {
            hitCheckTimer -= Time.fixedDeltaTime;
            attackTimer -= Time.fixedDeltaTime;
            
            if (hitCheckTimer <= 0) {
                TryShoot();
                hitCheckTimer = 0.5f;
            }
            return;
        }
        
        rigidbody.velocity = transform.forward * (calculatedThrust * Time.fixedDeltaTime);

        thrustTimer -= Time.fixedDeltaTime;
        if (thrustTimer <= 0) {
            thrustTimer = thrustRandomizationOffset;
            calculatedThrust = thrustPower * Random.Range(1, thrustRandomization);
        }
    }

    public void TryShoot() {
        if(attackTimer > 0) return;
        attackTimer = attackCooldown;
        
        // Deal Damage
        Debug.Log("A tank hit us!");
    }

    public bool IsInRange() {
        return (transform.position - target.position).sqrMagnitude < attackRange;
    }
}
