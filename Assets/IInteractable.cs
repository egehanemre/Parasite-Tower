using UnityEngine;

public interface IInteractable
{
    void Interact();
    string GetInteractText(); 
    bool CanGrab();
    bool CanHold(out float holdTime);
    void InteractOnHolding(Transform pov, float holdingTime);
    void SetPhysicsMode(bool isActive);
}
