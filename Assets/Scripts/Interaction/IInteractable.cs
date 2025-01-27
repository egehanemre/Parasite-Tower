using UnityEngine;

public interface IInteractable
{
    void Interact();
    string GetInteractText(); 
    bool CanGrab();
    void SetGrab(bool state) {}
    bool CanHold(out float holdTime);
    void InteractOnHolding(Transform pov, float holdingTime);
    void SetPhysicsMode(bool isActive);
}
