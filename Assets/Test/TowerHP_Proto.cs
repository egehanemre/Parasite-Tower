using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHP_Proto : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Tower HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            TowerDestroyed();
        }
    }

    void TowerDestroyed()
    {
        Debug.Log("Tower has been destroyed!");
    }

}
