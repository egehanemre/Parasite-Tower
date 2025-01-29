using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInteractionSystem : MonoBehaviour
{
    public float maxRayDistance = 4f;
    public LayerMask interactableLayer;
    public GameObject interactionButton;

    [Header("Old Logic Additions")]
    [SerializeField] private Transform grabLocation;
    [SerializeField] private float dropForce = 5f;

    private IInteractable currentInteractable;
    private IInteractable itemOnHand;
    private IInteractable holdingAt;
    private float holdingTimer;
    [SerializeField] private AudioClip onItemPickedSound;
    [SerializeField] private AudioClip onItemDropSound;

    private Transform pov;

    private void Start()
    {
        pov = Camera.main?.transform;
        if (pov == null) Debug.LogError("Main Camera not found! Ensure your main camera is tagged correctly.");
    }

    private void Update()
    {
        HandleInteraction();
        UpdateInteractionUI();
        HandleDropRequest();
    }

    private void HandleInteraction()
    {
        IInteractable itemOnFocus = GetItemOnFocus();
        currentInteractable = itemOnFocus;

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact(this);
            if (currentInteractable.CanGrab())
            {
                if (itemOnHand != null) DropItem();
                GrabItem(currentInteractable);
            }
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
        item.SetGrab(true);
        item.SetPhysicsMode(false);
        Transform itemTransform = ((MonoBehaviour)item).transform;
        itemTransform.SetParent(grabLocation);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
        itemOnHand = item;
        AudioSource.PlayClipAtPoint(onItemPickedSound, transform.position);
    }

    public void DropItem()
    {
        if (itemOnHand == null) return;

        itemOnHand.SetGrab(false);
        itemOnHand.SetPhysicsMode(true);
        Transform itemTransform = ((MonoBehaviour)itemOnHand).transform;
        itemTransform.SetParent(null);
        itemOnHand = null;

        if (itemTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(pov.forward * dropForce, ForceMode.Impulse);
        }
        AudioSource.PlayClipAtPoint(onItemDropSound, transform.position);
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
        if (currentInteractable != null) {
            interactionButton.SetActive(true);
        }
        else {
            interactionButton.SetActive(false);
        }
    }

    public GameObject GetItemOnHand()
    {
        if (itemOnHand == null) return null;
        MonoBehaviour monoBehaviour = itemOnHand as MonoBehaviour;
        return monoBehaviour.gameObject;
    }

    public IInteractable GetObjectsInteractable()
    {
        return currentInteractable as IInteractable;
    }
}
