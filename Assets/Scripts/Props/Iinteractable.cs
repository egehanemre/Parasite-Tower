using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable
{
    public void Interact();
    public void InteractWhileOnHand(Transform pov);
    public void SetPhysicsMode(bool isActive);

    public bool CanGrab()
    {
        return false;
    }
}
