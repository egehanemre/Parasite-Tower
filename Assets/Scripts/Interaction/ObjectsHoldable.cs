using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsHoldable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "Hold E to interact";
    [SerializeField] private float holdingTime;
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
    }

    public virtual void Interact() {
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
