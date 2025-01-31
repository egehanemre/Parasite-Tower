using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class RocketTank : MonoBehaviour
{
    public TankLevel level;
    public float generalProjectileSpeed = 10;
    public Transform firePoint;
    public float fireRate;
    public float moveSpeed;
    public float rotationSpeed;
    public float stopRange;
    public int health;
    private Rigidbody rb;
    [SerializeField] private int baseProjectileDamage = 0;
    private float fireTimer;

    [SerializeField] private GameObject onHitEffect;
    [SerializeField] private GameObject afterHitEffect;
    [SerializeField] private GameObject onDeathEffect;

    void Start()
    {
        stopRange += Random.Range(-2.5f, 2.5f);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; 

        if (TankManager.Instance == null)
        {
            Debug.LogError("RocketTank: No TankManager instance found!");
        }
    }

    void FixedUpdate()
    {
        if (TankManager.Instance?.sharedTarget != null)
        {
            Transform targetTower = TankManager.Instance.sharedTarget;
            float distanceToTower = Vector3.Distance(transform.position, targetTower.position);

            if (distanceToTower > stopRange)
            {
                MoveTowardsTarget(targetTower);
                RotateTowardsTarget(targetTower);
            }
            else
            {
                StopMoving();
                FireProjectile(targetTower);
            }
        }

        AdjustToTerrain(); 
    }

    void MoveTowardsTarget(Transform targetTower)
    {
        Vector3 direction = (targetTower.position - transform.position).normalized;
        direction.y = 0; 

        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
    }

    void RotateTowardsTarget(Transform targetTower)
    {
        Vector3 direction = (targetTower.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    void StopMoving()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    void FireProjectile(Transform targetTower)
    {
        fireTimer += Time.fixedDeltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0;

            Vector3 direction = TankManager.Instance.sharedTarget.position + TankManager.Instance.generalAimOffset - transform.position;
            GameObject projectile = Instantiate(
                TankManager.Instance.sharedProjectilePrefab,
                firePoint.position, Quaternion.LookRotation(direction)
            );

            RocketTankProjectile projectileScript = projectile.GetComponent<RocketTankProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Launch(baseProjectileDamage, generalProjectileSpeed);
            }
        }
    }

    public void DealDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Instantiate(onDeathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void DealDamage(int amount, Vector3 location)
    {
        DealDamage(amount);
        if (health <= 0) return;

        GameObject spawnedEffect = Instantiate(onHitEffect, transform);
        spawnedEffect.transform.position = location;
        StartCoroutine(PlaySmoke(spawnedEffect.transform.localPosition, 1));
    }

    public IEnumerator PlaySmoke(Vector3 location, float after)
    {
        yield return new WaitForSeconds(after);
        GameObject spawnedEffect = Instantiate(afterHitEffect, transform);
        spawnedEffect.transform.localPosition = location;
    }

    void AdjustToTerrain()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 5f, LayerMask.GetMask("Terrain")))
        {
            Vector3 targetPosition = rb.position;
            targetPosition.y = hit.point.y + 0.5f; 

            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * 5f));

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        }
    }
}

[Serializable]
public enum TankLevel
{
    Level1,
    Level2,
    Level3
}
