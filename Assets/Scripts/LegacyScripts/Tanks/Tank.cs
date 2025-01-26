using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class Tank : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float thrustPower = 1f;
    [SerializeField] private float thrustRandomization = 1.25f;
    [SerializeField] private float thrustRandomizationOffset = 0.35f;
    [SerializeField] private float progress = 0;
    
    [Header("Shooting")]
    [SerializeField] private float damage = 5;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 10;

    [Header("Obstacle Check")] 
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float checkDistance = 50;
    [SerializeField] private float obstacleCheckTime = 0.33f;
    private float obstacleCheckTimer = 0;
    
    private float thrustTimer = 0;
    private float attackTimer = 0;
    private TankTarget target;
    private float calculatedThrust = 0.55f;
    private float hitCheckTimer = 0;
    private SplineContainer path;
    private bool shouldMove = true;

    public UnityEvent<Tank> onTankDestroyed = new UnityEvent<Tank>();

    public void Setup(TankTarget targetInstance, SplineContainer path) {
        target = targetInstance;
        this.path = path;
    }

    public void FixedUpdate() {
        obstacleCheckTime += Time.fixedDeltaTime;
        if (obstacleCheckTime > obstacleCheckTimer) {
            UpdateShouldMove();
            obstacleCheckTime = 0;
        }
        
        if (progress < 0.95f && shouldMove) {
            progress += (calculatedThrust * Time.fixedDeltaTime);
            path.Evaluate(progress, out float3 position, out float3 tangent, out float3 up);
            transform.position = position;
            transform.LookAt(tangent+position);

            thrustTimer -= Time.fixedDeltaTime;
            if (thrustTimer <= 0)
            {
                thrustTimer = thrustRandomizationOffset;
                calculatedThrust = thrustPower * Random.Range(1, thrustRandomization);
            }
        }
        
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
    }

    private void UpdateShouldMove() {
        var transform1 = transform;
        bool hasObstacle = Physics.Raycast(transform1.position, transform1.forward, checkDistance, obstacleMask);
        shouldMove = !hasObstacle;
    }

    private void TryShoot() {
        if(attackTimer > 0) return;
        attackTimer = attackCooldown;
        
        // Deal Damage
        Debug.Log("A tank hit us!");
        target.ApplyDamage(damage);
    }

    private bool IsInRange() {
        return (transform.position - target.transform.position).sqrMagnitude < attackRange;
    }

    public void HitTank() {
        onTankDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
    
    
}
