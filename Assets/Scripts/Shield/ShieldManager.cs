using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    private List<Shield> allShields = new List<Shield>();
    [SerializeField] private int maxShieldAmount = 1;
    private List<Shield> activatedShields = new List<Shield>();

    private void Start() {
        allShields = FindObjectsByType<Shield>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void Activate(ShieldProtectionSide protectionSide) {
        foreach (var activatedShield in activatedShields) {
            if (activatedShield.protectionSide == protectionSide) {
                Debug.LogWarning("A shield of the same type is already activated");
                return;
            }
        }
        
        Shield matchedShield = null;
        foreach (var shield in allShields) {
            if (shield.protectionSide == protectionSide) {
                matchedShield = shield;
                break;
            }
        }

        if (matchedShield == null) {
            Debug.LogError("No shield with given direction id found");
            return;
        }
        
        while (activatedShields.Count+1 > maxShieldAmount) {
            activatedShields[0].gameObject.SetActive(false);
            activatedShields.RemoveAt(0);
        }
        
        activatedShields.Add(matchedShield);
        matchedShield.gameObject.SetActive(true);
    }
}
