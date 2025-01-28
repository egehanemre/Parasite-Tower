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
    private float fireTimer;

    [SerializeField] private GameObject onHitEffect;
    [SerializeField] private GameObject afterHitEffect;
    [SerializeField] private GameObject onDeathEffect;

    void Start()
    {
        stopRange += Random.Range(-2.5f, 2.5f);
        rb = GetComponent<Rigidbody>();

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
    }

    void MoveTowardsTarget(Transform targetTower)
    {
        Vector3 direction = (targetTower.position - transform.position).normalized;
        direction.y = 0;

        rb.velocity = direction * moveSpeed;
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
        rb.velocity = Vector3.zero;
    }

    void FireProjectile(Transform targetTower)
    {
        fireTimer += Time.fixedDeltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0;

            Vector3 direction = TankManager.Instance.sharedTarget.position+TankManager.Instance.generalAimOffset - transform.position;
            GameObject projectile = Instantiate(
                TankManager.Instance.sharedProjectilePrefab,
                firePoint.position, Quaternion.LookRotation(direction)
            );

            RocketTankProjectile projectileScript = projectile.GetComponent<RocketTankProjectile>();
            if (projectileScript != null) {
                projectileScript.Launch(1, generalProjectileSpeed);
            }
        }
    }

    public void DealDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameObject spawnedEffect = Instantiate(onDeathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    
    public void DealDamage(int amount, Vector3 location) {
        DealDamage(amount);
        if(health <= 0) return;
        
        GameObject spawnedEffect = Instantiate(onHitEffect, transform);
        spawnedEffect.transform.position = location;
        StartCoroutine(PlaySmoke(spawnedEffect.transform.localPosition, 1));
    }

    public IEnumerator PlaySmoke(Vector3 location, float after) {
        yield return new WaitForSeconds(after);
        GameObject spawnedEffect = Instantiate(afterHitEffect, transform);
        spawnedEffect.transform.localPosition = location;
        //spawnedEffect.transform.eulerAngles = spawnedEffect.transform.eulerAngles + new Vector3(180, 0, 0);
    }
}

[Serializable]
public enum TankLevel
{
    Level1,
    Level2,
    Level3
}