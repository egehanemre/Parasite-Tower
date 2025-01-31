using UnityEngine;

public class ObjectsInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Press E to interact";

    public virtual void Interact(PlayerInteractionSystem playerInteractionSystem)
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
        else if(gameObject.CompareTag("MonitoringStation"))
        {
            if(TryGetComponent<MonitoringStation>(out MonitoringStation monitoringStation))
            {
                monitoringStation.interactionCounter = monitoringStation.interactionCooldown;
                monitoringStation.IncreasePriority();
                monitoringStation.UnlockCursor();
            }
        }
        else
        {
            Debug.Log("no tag found");
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void SetPhysicsMode(bool isActive)
    {
    }

    public bool CanGrab()
    {
        return false;
    }
}
