using UnityEngine;

public class TankManager : MonoBehaviour
{
    public static TankManager Instance { get; private set; }  // Singleton

    public Transform sharedTarget; 
    public GameObject sharedProjectilePrefab;  

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (sharedTarget == null)
        {
            Debug.LogError("TankManager: No shared target assigned!");
        }

        if (sharedProjectilePrefab == null)
        {
            Debug.LogError("TankManager: No shared projectile prefab assigned!");
        }
    }
}
