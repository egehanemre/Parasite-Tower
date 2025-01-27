using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerInteractionSystem playerInteractionSystem);
    string GetInteractText(); 
    bool CanGrab();
    void SetGrab(bool state) {}
    void SetPhysicsMode(bool isActive);
}
