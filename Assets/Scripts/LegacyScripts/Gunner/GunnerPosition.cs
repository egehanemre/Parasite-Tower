using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GunnerPosition : MonoBehaviour, Iinteractable, IPowerDependent, IRadarTarget
{
    [SerializeField] private GameObject NightVisionObject;
    [SerializeField] private GameObject LensEffectObject;
    [SerializeField] private CinemachineVirtualCamera linkedCamera;
    public bool beingUsedByPlayer;
    public bool beingUsedByAI;
    private PlayerInteraction lastInteraction;
    private PlayerController controller;
    [SerializeField] private bool hasEnergy = true;
    [SerializeField] private Vector3 radarOffset = Vector3.zero;
    [SerializeField] private Sprite seatedSprite;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite electricShutdownIcon;

    [Header("Shooting")] 
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float gunRange;
    [SerializeField] private float viewportYToRotation;
    [SerializeField] private float viewportXToRotation;

    [Header("Aiming")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float horizontalEulerCap;
    [SerializeField] private float verticalEulerCap;
    [SerializeField] private float maxZoom = 80;
    [SerializeField] private float minZoom= 30;
    [SerializeField] private float zoomSpeed = 1;
    private float currentZoom = 60;
    private Vector3 movement;
    private Vector3 defaultEuler;
    private float currentVerticalEuler;
    private float currentHorizontalEuler;

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
        if (beingUsedByPlayer || beingUsedByAI) {
            Debug.Log("Gunner position is occupied.");
            return;
        }

        if (!hasEnergy) {
            Debug.Log("Gunner position has no energy");
            return;
        }

        lastInteraction = interaction;
        controller = lastInteraction.gameObject.GetComponent<PlayerController>();
        //controller.UpdateCameraMode(cameraLocation, null, true, cameraLocation.rotation);        
        controller.UpdateMovementLock(true);
        linkedCamera.gameObject.SetActive(true);
        beingUsedByPlayer = true;
        NightVisionObject.SetActive(true);
        LensEffectObject.SetActive(false);
        defaultEuler = linkedCamera.transform.eulerAngles;
    }

    public void Update() {
        if(!beingUsedByPlayer) return;
        movement = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0)*movementSpeed;
        currentVerticalEuler = Mathf.Clamp(currentVerticalEuler + movement.y, -verticalEulerCap, verticalEulerCap);
        currentHorizontalEuler = Mathf.Clamp(currentHorizontalEuler + movement.x, -horizontalEulerCap, horizontalEulerCap);
        linkedCamera.transform.eulerAngles =
            new Vector3(currentHorizontalEuler, currentVerticalEuler, 0) + defaultEuler;
        chamberTimer -= Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom + (-Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime), minZoom, maxZoom);
        linkedCamera.m_Lens.FieldOfView = currentZoom;
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //controller.UpdateCameraMode(controller.baseFollow, controller.baseLookAt, false);
            NightVisionObject.SetActive(false);
            LensEffectObject.SetActive(true);
            controller.UpdateMovementLock(false);
            linkedCamera.gameObject.SetActive(false);
            beingUsedByPlayer = false;
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

    public void SetPowerState(bool state) {
        hasEnergy = state;
        if(!beingUsedByPlayer || state == true) return;
        controller.UpdateMovementLock(false);
        linkedCamera.gameObject.SetActive(false);
        beingUsedByPlayer = false;
    }

    public bool GetPowerState() {
        return hasEnergy;
    }
    
    
    public bool ShouldRenderAtRadar() {
        return true;
    }

    public Vector3 RadarRenderOffset() {
        return radarOffset;
    }
    
    public Sprite GetRenderIcon() {
        if (!hasEnergy) return electricShutdownIcon;
        
        if (beingUsedByPlayer || beingUsedByAI) return seatedSprite;
        else return emptySprite;
    }

}
