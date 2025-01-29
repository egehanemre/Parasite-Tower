using UnityEngine;
using System.Collections;

public class RocketTankProjectile : TurretRocket
{
    
    protected override void Launch(int damage, float speed, Vector3 direction) {
        this.speed = speed;
        this.damage += damage;
    }
    
    protected override void FixedUpdate() {
        base.FixedUpdate();

        Vector3 velocity = transform.forward * (speed * Time.fixedDeltaTime);
        bool didHit = Physics.Raycast(transform.position, velocity, out RaycastHit hit, velocity.sqrMagnitude*velocityToRayDistance, hitMask);
        if (didHit && hit.collider.gameObject.TryGetComponent<TowerHealthManager>(out TowerHealthManager tower)) {
            tower.TakeDamage(damage);
            Explode();
        }
        else {
            transform.position += velocity;
        }
    }
    
    protected override void Explode() {
        LeaveTrailBehind();
        Destroy(gameObject);
    }
}
