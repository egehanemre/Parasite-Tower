using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public ShieldProtectionSide protectionSide;

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.TryGetComponent<RocketTankProjectile>(out RocketTankProjectile projectile)) {
            Destroy(projectile.gameObject);
        }
    }
}
