using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable
{
    public void Interact()
    {
        
    }

    public void InteractWhileOnHand(Transform pov)
    {
        
    }
    public void InteractWhileHolding(Transform pov, float holdingTime) { }

    public void SetPhysicsMode(bool isActive)
    {
        
    }

    public bool CanGrab() {
        return false;
    }

    public bool CanHold(out float time)
    {
        time = 0;
        return false;
    }
}
