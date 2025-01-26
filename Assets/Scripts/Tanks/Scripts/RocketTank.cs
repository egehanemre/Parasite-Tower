using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketTank : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 2f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stopRange = 2f;
    public int health = 1;

    private Rigidbody rb;
    private float fireTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        rb.useGravity = false;

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

            GameObject projectile = Instantiate(
                TankManager.Instance.sharedProjectilePrefab,
                firePoint.position,
                firePoint.rotation
            );

            RocketTankProjectile projectileScript = projectile.GetComponent<RocketTankProjectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetTower);
            }
        }
    }

    public void DealDamage(int amount)
    {
        health -= amount;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
