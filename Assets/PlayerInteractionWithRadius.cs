using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInteractionWithRadius : MonoBehaviour
{
    public float maxRayDistance = 4f;
    public LayerMask interactableLayer;
    public GameObject interactionButton;

    [Header("Old Logic Additions")]
    [SerializeField] private Transform grabLocation;
    [SerializeField] private float dropForce = 5f;
    [SerializeField] private float holdingTime = 0f;

    private IInteractable currentInteractable;
    private IInteractable itemOnHand;
    private IInteractable holdingAt;
    private float holdingTimer;

    private Transform pov;

    private void Start()
    {
        pov = Camera.main?.transform;
        if (pov == null) Debug.LogError("Main Camera not found! Ensure your main camera is tagged correctly.");
    }

    private void Update()
    {
        HandleInteraction();
        HandleHoldRequest();
        UpdateInteractionUI();
        HandleDropRequest();
    }

    private void HandleInteraction()
    {
        IInteractable itemOnFocus = GetItemOnFocus();

        if (itemOnFocus != null)
        {
            currentInteractable = itemOnFocus;

            if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
            {
                currentInteractable.Interact();

                if (currentInteractable.CanGrab())
                {
                    if (itemOnHand != null) DropItem();
                    GrabItem(currentInteractable);
                }
            }
        }
        else
        {
            currentInteractable = null;
        }
    }

    private void HandleHoldRequest()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (holdingAt == null && currentInteractable != null && currentInteractable.CanHold(out holdingTime))
            {
                holdingAt = currentInteractable;
            }

            if (holdingAt != null)
            {
                holdingTimer += Time.deltaTime;

                if (holdingTimer >= holdingTime)
                {
                    holdingAt.InteractOnHolding(pov, holdingTimer);
                    holdingAt = null;
                    holdingTimer = 0;
                }
            }
        }
        else
        {
            holdingTimer = 0;
            holdingAt = null;
        }
    }

    private void HandleDropRequest()
    {
        if (Input.GetKeyDown(KeyCode.G) && itemOnHand != null)
        {
            DropItem();
        }
    }

    public void GrabItem(IInteractable item)
    {
        item.SetPhysicsMode(false);
        Transform itemTransform = ((MonoBehaviour)item).transform;
        itemTransform.SetParent(grabLocation);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
        itemOnHand = item;
    }

    public void DropItem()
    {
        if (itemOnHand == null) return;

        itemOnHand.SetPhysicsMode(true);
        Transform itemTransform = ((MonoBehaviour)itemOnHand).transform;
        itemTransform.SetParent(null);
        itemOnHand = null;

        if (itemTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(pov.forward * dropForce, ForceMode.Impulse);
        }
    }

    private IInteractable GetItemOnFocus()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayer))
        {
            return hit.collider.GetComponent<IInteractable>();
        }
        return null;
    }

    private void UpdateInteractionUI()
    {
        if (currentInteractable != null)
        {
            interactionButton.SetActive(true);
        }
        else
        {
            interactionButton.SetActive(false);
        }
    }

    public ObjectsInteractable GetObjectsInteractable()
    {
        return currentInteractable as ObjectsInteractable;
    }
}
