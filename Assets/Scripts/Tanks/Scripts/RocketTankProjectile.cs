using UnityEngine;
using System.Collections;

public class RocketTankProjectile : TurretRocket
{
    protected override void FixedUpdate() {
        Vector3 velocity = transform.forward * (speed * Time.fixedDeltaTime);
        //Debug.DrawRay(transform.position, velocity*velocityToRayDistance, Color.red, Time.fixedDeltaTime);
        bool didHit = Physics.Raycast(transform.position, velocity, out RaycastHit hit, velocity.sqrMagnitude*velocityToRayDistance, hitMask);
        if (didHit && hit.collider.gameObject.TryGetComponent<TowerHealthManager>(out TowerHealthManager tower)) {
            tower.TakeDamage(damage);
            Explode();
        }
        else if (didHit && hit.collider.gameObject.TryGetComponent<Shield>(out Shield shield)) {
            Explode();
        }
        else {
            transform.position += velocity;
        }
        
        LifetimeCheck(Time.fixedDeltaTime);
    }
    
    protected override void Explode() {
        LeaveTrailBehind();                
        Destroy(gameObject);
    }
}
