using UnityEngine;
public class ObjectsGrabbable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Press E to grab";
    [SerializeField] private bool isGrabbed = false;

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

    public void SetPhysicsMode(bool isActive)
    {
        if (rb != null)
        {
            rb.isKinematic = !isActive; // Enable/disable physics
        }
    }

    public bool CanGrab()
    {
        return !isGrabbed; // This object can be grabbed
    }

    public void SetGrab(bool state) {
        isGrabbed = state;
    }

    public bool IsGrabbed() {
        return isGrabbed;
    }
}
