using TMPro;
using UnityEngine;

public class TowerHealthManager : MonoBehaviour
{
    public float maxHealth = 100f; 
    public TextMeshProUGUI healthText; 
    private float currentHealth;
    [SerializeField] private GameObject endGamePrefab;
    [SerializeField] private Transform ui;
    private bool alreadyEnd = false;

    void Start()
    {
        currentHealth = maxHealth; 
        UpdateHealthUI();
    }

    // Method to apply damage to the tower
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f && !alreadyEnd)
        {
            currentHealth = 0f;
            EndGame();
        }

        UpdateHealthUI();
    }
    private void EndGame() {
        alreadyEnd = true;
        GameObject spawnedObject = Instantiate(endGamePrefab, ui);
        spawnedObject.GetComponent<MainMenu>().StopGame();
    }
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Tower Health: " + currentHealth.ToString("0");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            RocketTankProjectile projectile = other.GetComponent<RocketTankProjectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.damage); 
                Destroy(other.gameObject); 
            }
        }
    }

}
