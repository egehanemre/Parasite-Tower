using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Turret : ObjectsInteractable
{
    [SerializeField] private GameObject NightVisionObject;
    [SerializeField] private GameObject LensEffectObject;
    [SerializeField] private CinemachineVirtualCamera linkedCamera;
    [SerializeField] private bool hasEnergy = true;
    [SerializeField] private bool canBeUsed = false;
    public bool beingUsedByPlayer;

    [Header("Shooting")] 
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float gunRange;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject projectilePrefab;

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
    private PlayerController controller;

    [Header("Reloading")]
    [SerializeField] private float reloadTime = 0.33f;
    private float chamberTimer = 0.33f;
    private bool ReadyToShoot => chamberTimer <= 0;

    [Header("Upgrades & Ai")] 
    private int level = 0;
    [SerializeField] private float shootingCooldown = 2;
    [SerializeField] private Transform shootingRange;
    [SerializeField] private Transform shootingTip;

    private void Awake()
    {
        defaultEuler = linkedCamera.transform.localEulerAngles;
    }

    public override void Interact() {
        if (!hasEnergy) {
            Debug.Log("Gunner position has no energy");
            return;
        }

        if (!canBeUsed)
        {
            Debug.Log("Gunner position cannot be used");
            return;
        }

        linkedCamera.gameObject.SetActive(true);
        beingUsedByPlayer = true;
        NightVisionObject.SetActive(true);
        LensEffectObject.SetActive(false);

        controller = FindObjectOfType<PlayerController>();
        if (controller)
        {
            controller.virtualCamera.transform.localEulerAngles = Vector3.zero;
            controller.UpdateMovementLock(true);
        }
    }

    public void Update() {
        if(!beingUsedByPlayer) return;
        movement = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0)*movementSpeed;
        currentVerticalEuler = Mathf.Clamp(currentVerticalEuler + movement.y, -verticalEulerCap, verticalEulerCap);
        currentHorizontalEuler = Mathf.Clamp(currentHorizontalEuler + movement.x, -horizontalEulerCap, horizontalEulerCap);
        linkedCamera.transform.localEulerAngles =
            new Vector3(currentHorizontalEuler, currentVerticalEuler, 0) + defaultEuler;
        chamberTimer -= Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom + (-Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime), minZoom, maxZoom);
        linkedCamera.m_Lens.FieldOfView = currentZoom;
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //controller.UpdateCameraMode(controller.baseFollow, controller.baseLookAt, false);
            NightVisionObject.SetActive(false);
            LensEffectObject.SetActive(true);
            linkedCamera.gameObject.SetActive(false);
            beingUsedByPlayer = false;
            
            if (controller) {
                controller.UpdateMovementLock(false);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && ReadyToShoot) {
            Shoot();
        }
    }

    private void Shoot() {
        Camera mainCam = Camera.main;
        if (!mainCam) {
            Debug.Log("No camera as start point found");
            return;
        }

        chamberTimer = reloadTime;
        InstantHitCheck(mainCam);
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

        if (hit.collider.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
            tank.DealDamage(damage);
            return;
        }
        
        Debug.Log("No tanks are hit");            
        Debug.DrawRay(targetRay.origin , targetRay.direction * 100, Color.red, 100, true);
        return;
    }

    public void LevelUp() {
        level++;

        if (level == 1) {
            canBeUsed = true;
        }
        else if (level == 2) {
            StopCoroutine(GunnerAILoop());
            StartCoroutine(GunnerAILoop());
        }
    }

    private IEnumerator GunnerAILoop()
    {
        while (level >= 2)
        {
            yield return new WaitForSeconds(shootingCooldown);
            if(beingUsedByPlayer) continue;
            
            Collider[] targets = Physics.OverlapBox(shootingRange.transform.position, shootingRange.lossyScale / 2, Quaternion.identity,
                targetMask);
            foreach (var target in targets) {
                if (target.TryGetComponent<RocketTank>(out RocketTank tank)) {
                    shootingTip.LookAt(tank.transform.position);
                    GameObject projectile = Instantiate(projectilePrefab);
                    projectile.transform.position = shootingTip.transform.position;
                    projectile.transform.rotation = shootingTip.transform.rotation;
                }
            }
        }
    }

}
