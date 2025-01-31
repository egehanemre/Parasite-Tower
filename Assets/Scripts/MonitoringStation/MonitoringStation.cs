using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitoringStation : MonoBehaviour
{
    public PlayerController playerController;
    public CinemachineVirtualCamera monitoringStationCamera;
    public List<GameObject> otherUIElements;
    public float interactionCooldown = 0.55f;
    public float interactionCounter = 0;

    void Start()
    {
        monitoringStationCamera.Priority = 1;
        playerController = FindObjectOfType<PlayerController>();
    }

    public void IncreasePriority()
    {
        monitoringStationCamera.Priority = 11;
        playerController.UpdateMovementLock(true);
        SetUIElementsActive(false);
        playerController.enabled = false;
    }

    public void DecreasePriority()
    {
        monitoringStationCamera.Priority = 1;
        playerController.UpdateMovementLock(false);
        SetUIElementsActive(true);
        playerController.enabled = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        interactionCounter -= Time.deltaTime;
        if (monitoringStationCamera.Priority == 11 && interactionCounter < 0)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E))
            {
                DecreasePriority();
                LockCursor();
            }
        }
    }

    private void SetUIElementsActive(bool isActive)
    {
        foreach (var element in otherUIElements)
        {
            element.SetActive(isActive);
        }
    }
}
