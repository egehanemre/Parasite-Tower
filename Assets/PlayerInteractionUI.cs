using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject interactableUIContainer;
    [SerializeField] private PlayerInteractionSystem playerInteraction;
    [SerializeField] private TextMeshProUGUI interactableTextGUI;

    private void Update()
    {
        IInteractable interactable = playerInteraction.GetObjectsInteractable();

        if (interactable != null)
        {
            Show(interactable);
        }
        else
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        if (interactable != null)
        {
            interactableTextGUI.text = interactable.GetInteractText();
            interactableUIContainer.SetActive(true);
        }
    }

    private void Hide()
    {
        interactableUIContainer.SetActive(false);
    }
}
