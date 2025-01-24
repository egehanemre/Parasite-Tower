using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour, Iinteractable
{
    [SerializeField] private LayerMask extinguishableLayer;    
    [SerializeField] private float extinguishRange = 5f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;
    [SerializeField] private float extinguishingCooldown = 0.33f;

    public void Interact() { }

    public void InteractWhileHolding(Transform pov, float holdingTime) {
        if(extinguishingCooldown > holdingTime) return;
        
        bool foundFire = Physics.Raycast(pov.transform.position, pov.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 
            extinguishRange, extinguishableLayer, QueryTriggerInteraction.Collide);

        if (foundFire && hit.collider.gameObject.TryGetComponent<Fire>(out Fire fire)) {
            fire.ChangeStage(-1);
        }
    }

    public void SetPhysicsMode(bool isActive)
    {
        collider.enabled = isActive;
        rigidbody.isKinematic = !isActive;
    }

    public bool CanGrab() {
        return true;
    }
}
