using UnityEngine;

public class ObjectsInteractable : MonoBehaviour, IInteractable
{
    public string interactText = "Press E to interact";
    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);   
    }
    public string GetInteractText()
    {
        return interactText;
    }

    public void InteractOnHolding(Transform pov, float holdingTime)
    {
        Debug.Log($"Holding {gameObject.name} for {holdingTime} seconds.");
    }

    public void SetPhysicsMode(bool isActive)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = !isActive;
        }
    }

    public bool CanHold(out float holdTime)
    {
        holdTime = 2f; // Example hold time
        return true;
    }

    public bool CanGrab()
    {
        return true;
    }
}
