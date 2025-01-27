using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitoringStation : MonoBehaviour
{
    public GameObject uiCanvas;
    private bool isUIActive = false;
    public PlayerController playerController;  

    void Start()
    {
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (isUIActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void TryInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            MonitoringStation station = hit.collider.GetComponent<MonitoringStation>();

            if (station != null && !isUIActive)
            {
                OpenUI();
            }
        }
    }

    private void OpenUI()
    {
        isUIActive = true;
        uiCanvas.SetActive(true);
        Time.timeScale = 0; 
        playerController.enabled = false;
        UnlockCursor();
    }

    private void CloseUI()
    {
        isUIActive = false;
        uiCanvas.SetActive(false);
        Time.timeScale = 1;
        playerController.enabled = true;

        LockCursor();

        StartCoroutine(ForceCursorLock());
    }
    private IEnumerator ForceCursorLock()
    {
        yield return null;

        Cursor.lockState = CursorLockMode.None; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }
}
