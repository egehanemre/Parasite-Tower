using UnityEngine;

public class ObjectsInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Press E to interact";

    public virtual void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);

        // Check if the GameObject has the specified tag
        if (gameObject.CompareTag("Turret"))
        {
            if (TryGetComponent<Turret>(out Turret turretScript))
            {
                turretScript.ActivateTurret();
            }
            else
            {
                Debug.LogWarning("Turret component not found on the GameObject!");
            }
        }
        else if (gameObject.CompareTag("ShieldButton"))
        {
            if (TryGetComponent<ShieldButton>(out ShieldButton shieldButton)) {
                shieldButton.ActivateLinkedSide();
            }
        }
        else
        {
            Debug.Log("This object is not a turret.");
        }
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
