using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsHoldable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Hold E to interact";
    [SerializeField] private float holdingTime;
    private PlayerInteractionSystem InteractionSystem;
    private bool holding = false;
    private float timeRemaining = 0;

    private void Update()
    {
        if (!Input.GetKey(KeyCode.E) && holding) {
            holding = false;
            timeRemaining = 0;
            HoldProgressBar.actionProgressBar.Render(false, 0);
        }

        if (Input.GetKey(KeyCode.E) && holding) {
            timeRemaining -= Time.deltaTime;
            float remainingProgress = (timeRemaining / holdingTime);
            HoldProgressBar.actionProgressBar.Render(true, remainingProgress);
            
            if (timeRemaining <= 0) {
                timeRemaining = 0;
                holding = false;
                OnHoldingComplete();
                HoldProgressBar.actionProgressBar.Render(false, 0);
            }
        }
    }

    private void OnHoldingComplete() {
        if (TryGetComponent<BodyRemainings>(out BodyRemainings bodyRemainings)) {
            bodyRemainings.Collect();
        }
        else if (TryGetComponent<SacrificePoint>(out SacrificePoint sacrificePoint)) {
            GameObject itemOnHand = InteractionSystem.GetItemOnHand();
            if (itemOnHand == null || !itemOnHand.CompareTag("Skull"))
            {
                Debug.Log("No valid items on hand for sacrifice");
                return;
            }
            
            InteractionSystem.DropItem();
            sacrificePoint.Sacrifice(itemOnHand);
        }
        else if (TryGetComponent<AmmunitionLoadChamber>(out AmmunitionLoadChamber loadChamber)) {
            GameObject itemOnHand = InteractionSystem.GetItemOnHand();
            if (itemOnHand == null || !itemOnHand.TryGetComponent<LoadableAmmunition>(out LoadableAmmunition loadableAmmunition)) {
                Debug.Log("No valid items on hand to load");
                return;
            }

            SpecialAmmunition ammunition = loadableAmmunition.GetAmmunition();
            loadChamber.FeedChamber(ammunition);
            InteractionSystem.DropItem();
            Destroy(loadableAmmunition.gameObject);
        }
    }

    public virtual void Interact(PlayerInteractionSystem playerInteractionSystem) {
        InteractionSystem = playerInteractionSystem;
        holding = true;
        timeRemaining = holdingTime;
        HoldProgressBar.actionProgressBar.Render(true, 1);
    }

    public string GetInteractText()
    {
        return interactText;
    }
    
    public void SetPhysicsMode(bool isActive) { }

    public bool CanGrab()
    {
        return false;
    }
}
