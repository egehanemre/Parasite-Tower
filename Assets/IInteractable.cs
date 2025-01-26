using UnityEngine;
public interface IInteractable
{
    void Interact();
    void InteractOnHolding(Transform pov, float holdingTime);
    void SetPhysicsMode(bool isActive);
    bool CanHold(out float holdTime);
    bool CanGrab();
}
