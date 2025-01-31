using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject enablingPart;
    private PlayerController playerController;
    [SerializeField] private bool CanEnableBack = true;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        volumeSlider.onValueChanged.AddListener(NewVolumeValue);
        restartButton.onClick.AddListener(RestartScene);
        if(CanEnableBack)continueButton.onClick.AddListener(Resume);
        volumeSlider.value = AudioListener.volume;
        playerController = FindObjectOfType<PlayerController>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale > 0) {
            StopGame();
        }
        else if(Input.GetKeyDown(KeyCode.P) && Mathf.Approximately(Time.timeScale, 0)) {
            if(CanEnableBack) Resume();
            else {
                RestartScene();
            }
        }
    }
    
    public void StopGame() {
        enablingPart.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(!playerController)playerController = FindObjectOfType<PlayerController>();
        playerController.UpdateMovementLock(true);
    }

    private void Resume() {
        enablingPart.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerController.UpdateMovementLock(false);
    }

    private void RestartScene() {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void NewVolumeValue(float value) {
        AudioListener.volume = value;
        volumeSlider.value = value;
    }
}
