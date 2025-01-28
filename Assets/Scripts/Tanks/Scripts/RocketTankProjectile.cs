using UnityEngine;
using System.Collections;

public class RocketTankProjectile : TurretRocket
{
    protected override void Explode() {
        Collider[] contacted = Physics.OverlapSphere(transform.position, explosionSize);
        foreach (var contact in contacted)
        {
            if (contact.gameObject.TryGetComponent<TowerHealthManager>(out TowerHealthManager tower)) {
                Destroy(gameObject);
            }
        }
    }
}
