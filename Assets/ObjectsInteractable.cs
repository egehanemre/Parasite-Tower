using UnityEngine;

public class ObjectsInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Press E to interact";

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
    }

    public void SetPhysicsMode(bool isActive)
    {
    }

    public bool CanHold(out float holdTime)
    {
        holdTime = 0f;
        return false; 
    }

    public bool CanGrab()
    {
        return false; 
    }
}
