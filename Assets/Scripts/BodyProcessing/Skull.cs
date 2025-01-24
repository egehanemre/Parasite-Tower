using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour, Iinteractable
{
    [SerializeField] private int pointsGain = 1;
    [SerializeField] private Collider collider;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float processingTime = 1;
    [SerializeField] private float processingRange = 30;
    private event Action OnSkullProcessed;

    public bool CanGrab() {
        return true;
    }
    
    public void InteractWhileHolding(Transform pov, float holdingTime) {
        if(processingTime > holdingTime) return;
        
        bool foundProcessor = Physics.Raycast(pov.transform.position, pov.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 
            processingRange, ~0, QueryTriggerInteraction.Collide);

        if (foundProcessor && hit.collider.gameObject.TryGetComponent<BodyProcessor>(out BodyProcessor processor)) {
            processor.Process(this);
            OnSkullProcessed?.Invoke();
            Destroy(gameObject);
        }
    }

    public Action GetOnDropEvent() {
        return OnSkullProcessed;
    }

    
    public void SetPhysicsMode(bool isActive)
    {
        collider.enabled = isActive;
        rigidbody.isKinematic = !isActive;
    }

    public int GetPoints()
    {
        return pointsGain;
    }
}
