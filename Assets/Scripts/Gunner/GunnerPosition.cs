using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GunnerPosition : MonoBehaviour, Iinteractable
{
    [SerializeField] private Transform cameraLocation;
    private bool beingUsed;
    private PlayerInteraction lastInteraction;
    private PlayerController controller;
    public void Interact(PlayerInteraction interaction) {
        if (beingUsed) {
            Debug.Log("Gunner position is occupied.");
            return;
        }

        lastInteraction = interaction;
        controller = lastInteraction.gameObject.GetComponent<PlayerController>();
        controller.UpdateCameraMode(cameraLocation, null, true, cameraLocation.rotation);
        beingUsed = true;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && beingUsed) {
            controller.UpdateCameraMode(controller.baseFollow, controller.baseLookAt, false);
            beingUsed = false;
        }
    }
}
