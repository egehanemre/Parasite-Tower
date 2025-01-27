using UnityEngine;
using Cinemachine;

public class Turret : MonoBehaviour
{
    [SerializeField] private UpgradeManager.UpgradeLevel turretLevel;
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

    [Header("Upgrade")]
    [SerializeField] private int damageIncreasePerLevel = 2;
    [SerializeField] private float rangeIncreasePerLevel = 10;

    private void Awake()
    {
        defaultEuler = linkedCamera.transform.localEulerAngles;
    }

    public void ActivateTurret()
    {
        if (!hasEnergy)
        {
            Debug.Log("Turret has no energy");
            return;
        }

        if (turretLevel <= UpgradeManager.UpgradeLevel.Level0)
        {
            Debug.Log("Turret cannot be used");
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

    private void Update()
    {
        if (!beingUsedByPlayer) return;

        // Turret movement and aiming logic
        movement = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * movementSpeed;
        currentVerticalEuler = Mathf.Clamp(currentVerticalEuler + movement.y, -verticalEulerCap, verticalEulerCap);
        currentHorizontalEuler = Mathf.Clamp(currentHorizontalEuler + movement.x, -horizontalEulerCap, horizontalEulerCap);
        linkedCamera.transform.localEulerAngles =
            new Vector3(currentHorizontalEuler, currentVerticalEuler, 0) + defaultEuler;
        chamberTimer -= Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom + (-Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime), minZoom, maxZoom);
        linkedCamera.m_Lens.FieldOfView = currentZoom;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit turret control
            NightVisionObject.SetActive(false);
            LensEffectObject.SetActive(true);
            linkedCamera.gameObject.SetActive(false);
            beingUsedByPlayer = false;

            if (controller)
            {
                controller.UpdateMovementLock(false);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && ReadyToShoot)
        {
            Shoot();
        }
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
        InstantHitCheck(mainCam);
    }

    private void InstantHitCheck(Camera mainCam)
    {
        Ray targetRay = mainCam.ScreenPointToRay(Input.mousePosition);
        bool foundObject = Physics.Raycast(targetRay, out RaycastHit hit, gunRange, targetMask);

        if (!foundObject)
        {
            Debug.Log("No objects are hit");
            Debug.DrawRay(targetRay.origin, targetRay.direction * 100, Color.red, 100, true);
            return;
        }

        if (hit.collider.gameObject.TryGetComponent<RocketTank>(out RocketTank tank))
        {
            tank.DealDamage(damage);
            return;
        }

        Debug.Log("No tanks are hit");
        Debug.DrawRay(targetRay.origin, targetRay.direction * 100, Color.red, 100, true);
        return;
    }

    public void LevelUp()
    {
        if (turretLevel >= UpgradeManager.UpgradeLevel.Level2)
        {
            Debug.Log("Turret is already at max level!");
            return;
        }

        turretLevel++;
        damage += damageIncreasePerLevel;
        gunRange += rangeIncreasePerLevel;

        Debug.Log($"Turret leveled up to Level {turretLevel}!");
        Debug.Log($"New Damage: {damage}, New Gun Range: {gunRange}");
    }
}
