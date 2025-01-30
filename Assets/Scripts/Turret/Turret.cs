using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class Turret : MonoBehaviour
{
    [SerializeField] private UpgradeManager.UpgradeLevel turretLevel;
    [SerializeField] private GameObject NightVisionObject;
    [SerializeField] private CinemachineVirtualCamera linkedCamera;
    [SerializeField] private bool hasEnergy = true;
    public bool beingUsedByPlayer;
    [SerializeField] private AudioClip audioOnSeated;
    private float seatedRecently = 0;

    [Header("Shooting")]
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform projectileLaunchLocation;
    [SerializeField] private float generalProjectileSpeed = 10;
    [SerializeField] private GameObject defaultProjectileType;

    [Header("Aiming")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float horizontalEulerCap;
    [SerializeField] private float verticalEulerCap;
    [SerializeField] private float maxZoom = 80;
    [SerializeField] private float minZoom = 30;
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
    private float currentReloadTime => reloadTime + ((int)turretLevel * reloadSpeedChangePerLevel);

    [Header("Upgrade")]
    [SerializeField] private int damageIncreasePerLevel = 2;
    [SerializeField] private float rangeIncreasePerLevel = 10;
    [SerializeField] private float reloadSpeedChangePerLevel = -0.05f;
    [SerializeField] private SpecialAmmunition loadedSpecialAmmunition;

    [Header("AI")] 
    [SerializeField] private float aIShootingCooldown = 5;
    [SerializeField] private float aICooldownChangePerLevel = -1;
    [SerializeField] private Transform aITargetZone;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float gunRange = 10;
    private float timePassed = 0;
    private float calculatedAIShootingCooldown => (aIShootingCooldown + ((int)turretLevel) * aICooldownChangePerLevel);

    private void Awake() {
        defaultEuler = linkedCamera.transform.localEulerAngles;
    }
    
    public void ActivateTurret()
    {
        if (!hasEnergy)
        {
            Debug.Log("Turret has no energy");
            return;
        }

        /*if (turretLevel <= UpgradeManager.UpgradeLevel.Level0)
        {
            Debug.Log("Turret cannot be used");
            return;
        }*/

        linkedCamera.gameObject.SetActive(true);
        beingUsedByPlayer = true;
        NightVisionObject.SetActive(true);

        controller = FindObjectOfType<PlayerController>();
        if (controller)
        {
            controller.virtualCamera.transform.localEulerAngles = Vector3.zero;
            controller.UpdateMovementLock(true);
        }
        
        AudioSource.PlayClipAtPoint(audioOnSeated, transform.position);
        SoundEffectsManager.instance.SetEffectPackActivation("gunner", true);
        SoundEffectsManager.instance.SetEffectPackActivation("tower", false);
        HoldProgressBar.actionProgressBar.Render(true, chamberTimer / currentReloadTime);
        seatedRecently = 0.15f;
    }

    private void Update() {
        chamberTimer -= Time.deltaTime;
        seatedRecently -= Time.deltaTime;
        AIShootingTick();
        if (!beingUsedByPlayer) return;
        if (chamberTimer > 0) {
            HoldProgressBar.actionProgressBar.Render(true, chamberTimer / currentReloadTime);
        }
        else {
            HoldProgressBar.actionProgressBar.Render(false, 0);
        }

        // Turret movement and aiming logic
        movement = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * movementSpeed;
        currentVerticalEuler = Mathf.Clamp(currentVerticalEuler + movement.y, -verticalEulerCap, verticalEulerCap);
        currentHorizontalEuler = Mathf.Clamp(currentHorizontalEuler + movement.x, -horizontalEulerCap, horizontalEulerCap);
        linkedCamera.transform.localEulerAngles =
            new Vector3(currentHorizontalEuler, currentVerticalEuler, 0) + defaultEuler;
        currentZoom = Mathf.Clamp(currentZoom + (-Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime), minZoom, maxZoom);
        linkedCamera.m_Lens.FieldOfView = currentZoom;

        if (seatedRecently < 0 && Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E)) {
            LeaveTurret();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && ReadyToShoot) {
            Shoot();
        }
    }

    private void LeaveTurret() {
        NightVisionObject.SetActive(false);
        linkedCamera.gameObject.SetActive(false);
        beingUsedByPlayer = false;
        HoldProgressBar.actionProgressBar.Render(false, 0);

        if (controller) {
            controller.UpdateMovementLock(false);
        }
        
        SoundEffectsManager.instance.SetEffectPackActivation("gunner", false);
        SoundEffectsManager.instance.SetEffectPackActivation("tower", true);
    }

    private void Shoot()
    {
        Camera mainCam = Camera.main;
        if (!mainCam)
        {
            Debug.Log("No camera as start point found");
            return;
        }

        chamberTimer = reloadTime;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        GameObject currentProjectileType = defaultProjectileType;
        if (loadedSpecialAmmunition.amount > 0) {
            loadedSpecialAmmunition.amount--;
            currentProjectileType = loadedSpecialAmmunition.ammunitionPrefab;
        }
        
        LaunchProjectile(currentProjectileType, ray.direction);
    }

    private void LaunchProjectile(GameObject projectilePrefab, Vector3 direction) {

        GameObject spawnedProjectile = Instantiate(projectilePrefab, projectileLaunchLocation.position,
            Quaternion.LookRotation(direction));
        if (spawnedProjectile.TryGetComponent<TurretProjectile>(out TurretProjectile turretProjectile)) {
            turretProjectile.Launch(damage, generalProjectileSpeed);
        }
        else {
            Debug.LogError("Projectile has no turret projectile script!");
            Destroy(spawnedProjectile);
        }
    }
    
    public void LevelUp()
    {
        if (turretLevel >= UpgradeManager.UpgradeLevel.Tier4) {
            Debug.Log("Turret is already at max level!");
            return;
        }

        turretLevel++;
        damage += damageIncreasePerLevel;
        gunRange += rangeIncreasePerLevel;
        

        Debug.Log($"Turret leveled up to Level {turretLevel}!");
        Debug.Log($"New Damage: {damage}");
    }

    public void LoadAmmo(SpecialAmmunition specialAmmunition) {
        loadedSpecialAmmunition = specialAmmunition;
    }

    public void AIShootingTick() {
        timePassed += Time.deltaTime;
        if(timePassed < calculatedAIShootingCooldown) return;
        timePassed = 0;
        if(beingUsedByPlayer || !hasEnergy) return;
            
        Collider[] allTargets = Physics.OverlapBox(aITargetZone.position, aITargetZone.lossyScale / 2, aITargetZone.rotation, targetMask);
        foreach (var target in allTargets) {
            float distance = (target.transform.position - transform.position).sqrMagnitude;
            if(distance > gunRange) continue;
            
            if (target && target.gameObject.TryGetComponent<RocketTank>(out RocketTank tank)) {
                Vector3 direction = tank.transform.position - projectileLaunchLocation.position;
                LaunchProjectile(defaultProjectileType, direction);
                break;
            }
        }
    }


    public void CutEnergy() {
        if(!hasEnergy) return;
        hasEnergy = false;
        if(beingUsedByPlayer)LeaveTurret();
    }

    public void RecoverEnergy() {
        if(hasEnergy) return;
        hasEnergy = true;
    }
}
