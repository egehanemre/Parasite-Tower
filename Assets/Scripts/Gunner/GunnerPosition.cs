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

    [Header("Shooting")] 
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float gunRange;

    [Header("Reloading")]
    [SerializeField] private float reloadTime = 0.33f;
    private float chamberTimer = 0.33f;
    private bool ReadyToShoot => chamberTimer <= 0;


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
        if(!beingUsed) return;
        chamberTimer -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            controller.UpdateCameraMode(controller.baseFollow, controller.baseLookAt, false);
            beingUsed = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && ReadyToShoot) {
            ShootProjectile();
        }
    }

    public void ShootProjectile() {
        Camera mainCam = Camera.main;
        if (!mainCam) {
            Debug.Log("No camera as start point found");
            return;
        }
        
        chamberTimer = reloadTime;
        
        Ray targetRay = mainCam.ScreenPointToRay(Input.mousePosition);
        bool foundObject = Physics.Raycast(targetRay, out RaycastHit hit, gunRange, targetMask);
        //bool foundObject = Physics.Raycast(cameraLocation.position, targetPosition, out RaycastHit hit, gunRange);

        if (!foundObject) {
            Debug.Log("No objects are hit");
            Debug.DrawRay(targetRay.origin , targetRay.direction * 100, Color.red, 100, true);
            return;
        }

        if (hit.collider.gameObject.TryGetComponent<Tank>(out Tank tank)) {
            tank.HitTank();
            return;
        }

        
        Debug.Log("No tanks are hit");            
        Debug.DrawRay(targetRay.origin , targetRay.direction * 100, Color.red, 100, true);
        return;
    }
}
