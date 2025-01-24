using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyProcessor : MonoBehaviour, Iinteractable
{
    [SerializeField] private int currentPoints = 0;
    [SerializeField] private float processingTime = 2;
    
    public void Process(Skull skull) {
        int pointsToGain = skull.GetPoints();
        currentPoints += pointsToGain;
    }
    
    public bool CanHold(out float time) {
        time = processingTime;
        return true;
    }

    public int GetPoints() {
        return currentPoints;
    }

    public bool TryWithdraw(int amountOfPoints) {
        if (amountOfPoints <= currentPoints) {
            currentPoints -= amountOfPoints;
            return true;
        }

        return false;
    }
}
