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
    [SerializeField] private Vector3 radarOffset = Vector3.zero;
    [SerializeField] private Sprite playerIcon;

    [Header("Holding")]
    [SerializeField] private float holdingTime = 0;
    private Iinteractable holdingAt;
    private float holdingTimer;

    private void Start()
    {
        if(pov == null) pov = Camera.main.transform;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            holdingAt = GetItemOnFocus();
            itemOnHand?.InteractWhileOnHand(pov);
            OnInteractionRequest();
        } 
        
        else if (Input.GetKey(KeyCode.E)) OnHoldRequest();
        else if (holdingAt != null) holdingTimer = 0;
        if (Input.GetKey(KeyCode.G)) if(itemOnHand != null) DropItem();
    }

    private void UpdateSubscription(Iinteractable interactable, bool newState) {
        Action subscription = interactable.GetOnDropEvent();
        if (subscription == null) return;

        if (newState) {
            subscription += DropItem;
        }
        else {
            subscription -= DropItem;
        }
    }

    private void OnHoldRequest() {
        if(holdingAt == null) return;
        
        if (!holdingAt.CanHold(out holdingTime)) {
            holdingAt = null;
        }
        else  {
            holdingTimer += Time.deltaTime;
            if (holdingTime <= holdingTimer) {
                holdingAt?.InteractOnHolding(pov,holdingTimer);
                holdingAt = null;
                itemOnHand?.InteractWhileHolding(pov, holdingTimer);
                holdingTimer = 0;
            }
        }
    }

    public void OnInteractionRequest()
    {
        Iinteractable itemOnFocus = GetItemOnFocus();
        if (itemOnFocus == null) return;
        itemOnFocus.Interact(this);
                
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
        UpdateSubscription(itemOnHand, true);
    }

    public void DropItem() {
        itemOnHand.SetPhysicsMode(true);
        MonoBehaviour itemMn = itemOnHand as MonoBehaviour;
        itemMn.transform.SetParent(null);
        UpdateSubscription(itemOnHand, false);
        itemOnHand = null;

        if (itemMn.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.AddForce(pov.forward * dropForce, ForceMode.Impulse);
        }
    }

    public Iinteractable GetItemOnFocus() {
        bool foundObject = Physics.Raycast(pov.transform.position, pov.transform.TransformDirection(Vector3.forward), out RaycastHit hit,
            interactionRange, interactableLayer, QueryTriggerInteraction.Collide);
        Debug.DrawRay(pov.transform.position, pov.transform.TransformDirection(Vector3.forward) * interactionRange, Color.blue, 1);
        
        if (foundObject) {
            return hit.collider.GetComponent<Iinteractable>();
        }

        return null;
    }
    
    
    
    //public bool ShouldRenderAtRadar() {
    //    return true;
    //}

    //public Sprite GetRenderIcon() {
    //    return playerIcon;
    //}

}
