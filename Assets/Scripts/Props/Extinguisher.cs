//using System;
//using UnityEngine;

//public class Extinguisher : MonoBehaviour, IInteractable
//{
//    [SerializeField] private LayerMask extinguishableLayer; 
//    [SerializeField] private float extinguishRange = 5f; 
//    [SerializeField] private Rigidbody rigidbody; 
//    [SerializeField] private Collider collider; 
//    [SerializeField] private float extinguishingCooldown = 0.33f; 
//    public void Interact()
//    {
//        Debug.Log("Interacted with Extinguisher!");
//    }
//    public void InteractOnHolding(Transform pov, float holdingTime)
//    {
//        if (holdingTime < extinguishingCooldown) return;

//        if (Physics.Raycast(
//                pov.position,
//                pov.forward,
//                out RaycastHit hit,
//                extinguishRange,
//                extinguishableLayer,
//                QueryTriggerInteraction.Collide))
//        {
//            if (hit.collider.gameObject.TryGetComponent<Fire>(out Fire fire))
//            {
//                fire.ChangeStage(-1);
//                Debug.Log("Fire extinguished!");
//            }
//        }
//    }

//    public void SetPhysicsMode(bool isActive)
//    {
//        if (collider != null) collider.enabled = isActive;
//        if (rigidbody != null) rigidbody.isKinematic = !isActive;
//    }

//    public bool CanGrab()
//    {
//        return true;
//    }

//    public bool CanHold(out float holdTime)
//    {
//        holdTime = 0f;
//        return false; 
//    }
//}
