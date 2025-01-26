using UnityEngine;
using System.Collections;

public class RocketTankProjectile : MonoBehaviour
{
    public Transform target;
    public float projectileSpeed = 2f;
    public float upwardForce = 5f;
    public float deviationAngle = 5f;
    public float acceleration = 0.1f;
    public float damageAmount = 10f; // Damage amount the projectile deals

    private Vector3 startPoint;
    private Rigidbody rb;
    private bool isLaunched = false;

    void Start()
    {
        startPoint = transform.position;
        rb = GetComponent<Rigidbody>();

        if (target == null)
        {
            Debug.LogError("Target not assigned to the projectile!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isLaunched && target != null)
        {
            LaunchProjectile();
            isLaunched = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            Destroy(gameObject);
        }
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isLaunched = false;
    }

    void LaunchProjectile()
    {
        Vector3 directionToTarget = target.position - startPoint;

        Vector3 deviation = new Vector3(
            Random.Range(-deviationAngle, deviationAngle),
            Random.Range(-deviationAngle, deviationAngle),
            Random.Range(-deviationAngle, deviationAngle)
        );

        directionToTarget = Quaternion.Euler(deviation) * directionToTarget;

        directionToTarget.Normalize();

        Vector3 launchVelocity = directionToTarget * projectileSpeed;

        rb.velocity = launchVelocity;

        StartCoroutine(AccelerateProjectile());
    }

    private IEnumerator AccelerateProjectile()
    {
        while (true)
        {
            rb.velocity += rb.velocity.normalized * acceleration * Time.deltaTime;
            yield return null;
        }
    }
}
