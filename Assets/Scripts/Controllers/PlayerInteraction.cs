using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform pov;
    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private Iinteractable itemOnHand;
    [SerializeField] private Transform grabLocation;
    [SerializeField] private float dropForce = 5f;

    private void Start()
    {
        if(pov == null) pov = Camera.main.transform;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            itemOnHand?.InteractWhileOnHand(pov);
            OnInteractionRequest();
        }

        if (Input.GetKey(KeyCode.G))
        {
            if(itemOnHand != null) DropItem();
        }
    }

    public void OnInteractionRequest()
    {
        Iinteractable itemOnFocus = GetItemOnFocus();
        if (itemOnFocus == null) return;
        
        itemOnFocus.Interact();
                
        if (itemOnFocus.CanGrab()) {
            if(itemOnHand != null) DropItem();
            GrabItem(itemOnFocus);
        }
    }

    public void GrabItem(Iinteractable item) {
        MonoBehaviour itemMn = item as MonoBehaviour;
        item.SetPhysicsMode(false);
        Transform itemTransform = itemMn.transform; 
        itemMn.transform.SetParent(grabLocation);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
        itemOnHand = item;
    }

    public void DropItem() {
        itemOnHand.SetPhysicsMode(true);
        MonoBehaviour itemMn = itemOnHand as MonoBehaviour;
        itemMn.transform.SetParent(null);
        itemOnHand = null;

        if (itemMn.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.AddForce(pov.forward * dropForce, ForceMode.Impulse);
        }
    }

    public Iinteractable GetItemOnFocus() {
        bool foundObject = Physics.Raycast(pov.transform.position, pov.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 
            interactionRange, interactableLayer, QueryTriggerInteraction.Collide);

        if (foundObject) {
            return hit.collider.GetComponent<Iinteractable>();
        }

        return null;
    }
}
