using UnityEngine;

public interface IInteractable
{
    void Interact();
    string GetInteractText(); 
    bool CanGrab();
    void SetGrab(bool state) {}
    void SetPhysicsMode(bool isActive);
}
