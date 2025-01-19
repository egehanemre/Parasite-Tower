using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    [SerializeField] private TowerShield[] shields;
    [SerializeField] private ShieldLever[] levers;

    public void Start() {
        shields = FindObjectsByType<TowerShield>(FindObjectsSortMode.None);
        levers = FindObjectsByType<ShieldLever>(FindObjectsSortMode.None);
        ListenLevers();
    }

    private void ListenLevers() {
        if (levers == null || levers.Length == 0) {
            Debug.Log("No levers detected");
            return;
        }
        
        foreach (var lever in levers) {
            lever.onLeverPulled.RemoveAllListeners();
            lever.onLeverPulled.AddListener(OnLeverPulled);
        }
    }

    private void OnLeverPulled(string side) {
        if (shields == null || shields.Length == 0) {
            Debug.Log("No shields detected for lever pull");
            return;
        }
        
        ReRenderLevers(side);

        foreach (var shield in shields) {
            shield.Turn(shield.MatchesSide(side));
        }
    }

    private void ReRenderLevers(string side)
    {
        if (levers == null || levers.Length == 0) {
            Debug.Log("No levers detected");
            return;
        }
        
        foreach (var lever in levers) {
            if(lever.MatchesSide(side)) lever.UpdateVisualizer(true);
            else lever.UpdateVisualizer(false);
        } 
    }
    
}
