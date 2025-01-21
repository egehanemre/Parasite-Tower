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
    private bool hasEnergy = true;

    [Header("Shooting")] 
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float gunRange;
    [SerializeField] private float viewportYToRotation;
    [SerializeField] private float viewportXToRotation;
    
    [Header("Projectiles")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private List<GameObject> activeProjectiles = new List<GameObject>();
    [SerializeField] private Transform projectileSpawnLocation;
    [SerializeField] private float projectileTravelSpeed = 1;
    [SerializeField] private float projectileGravity = 0.2f;
    [SerializeField] private LayerMask projectileHitMask;

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
        
        TickProjectiles(Time.deltaTime);
    }

    private void TickProjectiles(float deltaTime) {
        if (activeProjectiles == null || activeProjectiles.Count == 0) {
            // No debug log because it's going to happen frequently
            return;
        }
        
        for (int i = 0; i < activeProjectiles.Count; i++) {
            Transform currentProjectileTransform = activeProjectiles[i].transform;

            bool didHit = Physics.Raycast(currentProjectileTransform.position, currentProjectileTransform.forward, out RaycastHit hit, projectileTravelSpeed * deltaTime, projectileHitMask);
            if (didHit && hit.collider.gameObject.TryGetComponent<Tank>(out Tank tank)) {
                tank.HitTank();
                Destroy(activeProjectiles[i]);
                activeProjectiles.RemoveAt(i);
                i--;
                continue;
            } else if (didHit) {
                Destroy(activeProjectiles[i]);
                activeProjectiles.RemoveAt(i);
                i--;
            }

            currentProjectileTransform.position += currentProjectileTransform.forward * (projectileTravelSpeed * deltaTime);
            currentProjectileTransform.eulerAngles += new Vector3(projectileGravity * deltaTime, 0, 0);
        }
    }

    private void ShootProjectile() {
        Camera mainCam = Camera.main;
        if (!mainCam) {
            Debug.Log("No camera as start point found");
            return;
        }

        if (!projectilePrefab || !projectileSpawnLocation) {
            Debug.Log("Can't spawn projectile");
            return;
        }
        
        chamberTimer = reloadTime;
        //ViewportBasedProjectile(mainCam);
        InstantHitCheck(mainCam);
    }

    private void ViewportBasedProjectile(Camera mainCam) {
        Vector3 viewportPoint = mainCam.ScreenToViewportPoint(Input.mousePosition);
        GameObject spawnedProjectile = Instantiate(projectilePrefab);
        spawnedProjectile.transform.position = projectileSpawnLocation.position;
        Vector3 newEuler = new Vector3((viewportYToRotation * (viewportPoint.y-0.5f))*2, -90 + (((viewportPoint.x-0.5f)) * viewportXToRotation)*2, 0);
        spawnedProjectile.transform.localEulerAngles = newEuler;
        activeProjectiles.Add(spawnedProjectile);
    }

    private void InstantHitCheck(Camera mainCam) {
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
