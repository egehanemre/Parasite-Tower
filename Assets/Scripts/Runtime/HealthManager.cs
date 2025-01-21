using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    private bool DEBUG = false;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private TowerShield[] shields;
    [SerializeField] private TankTarget[] sides;
    [SerializeField] private HealthBar healthBar;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F8)) {
            DEBUG = !DEBUG;
        }
    }

    public void Start() {
        shields = FindObjectsByType<TowerShield>(FindObjectsSortMode.None);
        healthBar = FindObjectOfType<HealthBar>(true);
        RegisterToDamages();
    }

    public void RegisterToDamages() {
        sides = FindObjectsByType<TankTarget>(FindObjectsSortMode.None);

        if (sides == null || sides.Length == 0) {
            Debug.Log("No sides detected");
            return;
        }

        foreach (var side in sides) {
            side.onDamageTaken.RemoveAllListeners();
            side.onDamageTaken.AddListener(OnDamageTaken);
        }
    }

    public void OnDamageTaken(float damage, string side) {
        if (shields == null || shields.Length == 0) {
            Debug.Log("No proper shield registered");
            health -= damage;
            return;
        }
        
        foreach (var shield in shields) {
            if (shield.DoesProtect(side)) {
                Debug.Log("A shield absorbed an attack!");
                return;
            }
        }

        Debug.Log("Damage taken.. " + health + " => " + (health-damage));
        health -= damage;
        if(healthBar)healthBar.UpdateValue(health/maxHealth);

        if (health <= 0 && !DEBUG) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
