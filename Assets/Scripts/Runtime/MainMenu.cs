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

    private void Awake() {
        instance = this;
    }

    private void Start() {
        volumeSlider.onValueChanged.AddListener(NewVolumeValue);
        restartButton.onClick.AddListener(RestartScene);
        continueButton.onClick.AddListener(Resume);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && Time.timeScale > 0) {
            StopGame();
        }
    }
    
    public void StopGame() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void Resume() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void NewVolumeValue(float value) {
        AudioListener.volume = value;
        volumeSlider.value = value;
    }
}
