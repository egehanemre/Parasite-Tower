using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape : MonoBehaviour, Iinteractable
{
    [SerializeField] private LayerMask fixableLayer;    
    [SerializeField] private float fixingRange = 5f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;
    [SerializeField] private float fixingCooldown = 0.33f;

    public void Interact() { }

    public void InteractWhileHolding(Transform pov, float holdingTime) {
        if(fixingCooldown > holdingTime) return;
        
        bool foundPower = Physics.Raycast(pov.transform.position, pov.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 
            fixingRange, fixableLayer, QueryTriggerInteraction.Collide);

        if (foundPower && hit.collider.gameObject.TryGetComponent<PowerSupply>(out PowerSupply powerSupply)) {
            powerSupply.Fix();
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
    
    public bool CanHold(out float time)
    {
        time = fixingCooldown;
        return true;
    }
}
