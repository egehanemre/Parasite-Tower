using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 10;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed = 2;

    private void Awake()
    {
        rigidbody.velocity = transform.forward * speed;
    }

    private void FixedUpdate() {
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
            tank.DealDamage(1);
            Destroy(gameObject);
        }
    }
}
