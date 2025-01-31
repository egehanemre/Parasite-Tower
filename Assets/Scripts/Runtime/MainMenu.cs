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

    private void Awake() {
        instance = this;
    }

    private void Start() {
        volumeSlider.onValueChanged.AddListener(NewVolumeValue);
        restartButton.onClick.AddListener(RestartScene);
        continueButton.onClick.AddListener(Resume);
        volumeSlider.value = AudioListener.volume;
        playerController = FindObjectOfType<PlayerController>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0) {
            StopGame();
        }
    }
    
    public void StopGame() {
        enablingPart.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
