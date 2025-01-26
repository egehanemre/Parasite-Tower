using UnityEngine;
public class ObjectsGrabbable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Press E to grab";
    [SerializeField] private float holdTime = 1.5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        Debug.Log("Grabbed: " + gameObject.name);
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void InteractOnHolding(Transform pov, float holdingTime)
    {
        Debug.Log($"Holding {gameObject.name} for {holdingTime} seconds.");
    }

    public void SetPhysicsMode(bool isActive)
    {
        if (rb != null)
        {
            rb.isKinematic = !isActive; // Enable/disable physics
        }
    }

    public bool CanHold(out float holdTime)
    {
        holdTime = this.holdTime;
        return true; // This object can be held
    }

    public bool CanGrab()
    {
        return true; // This object can be grabbed
    }
}
