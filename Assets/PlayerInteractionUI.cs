using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject interactableUIContainer;
    [SerializeField] private PlayerInteractionWithRadius playerInteractionWithRadius;
    [SerializeField] private TextMeshProUGUI interactableTextGUI;

    private void Update()
    {
        ObjectsInteractable interactable = playerInteractionWithRadius.GetObjectsInteractable();

        if (interactable != null)
        {
            Show(interactable);
        }
        else
        {
            Hide();
        }
    }

    private void Show(ObjectsInteractable objectsInteractable)
    {
        if (objectsInteractable != null)
        {
            interactableTextGUI.text = objectsInteractable.GetInteractText();
            interactableUIContainer.SetActive(true);
        }
    }

    private void Hide()
    {
        interactableUIContainer.SetActive(false);
    }
}
