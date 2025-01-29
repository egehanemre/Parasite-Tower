using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public CinemachineVirtualCamera virtualCamera;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float sprintTransitSpeed = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float maxLookAngle = 70f;
    private float verticalVelocity;
    private float currentSpeed;
    private float xRotation;
    private bool movementLocked;

    [Header("Input")]
    [SerializeField] private float mouseSensitivity = 1f;
    private float moveInput;
    private float strafeInput;
    private float mouseX;
    private float mouseY;

    [Header("Camera Bob Settings")]
    [SerializeField] private float bobFrequency = 1f;
    [SerializeField] private float bobAmplitude = 1f;
    private float currentBobAmplitude = 0f;
    private float currentBobFrequency = 0f;
    [SerializeField] private float bobTransitionSpeed = 2f;
    private float bobTimer = 0f;
    private CinemachineBasicMultiChannelPerlin noiseComponent;
    private bool cameraBobLocked;

    [Header("Movement Sound")] 
    [SerializeField] private float progressToStepRate = 1;
    [SerializeField] private List<AudioClip> stepSounds = new List<AudioClip>();
    private int stepSoundIndex = 0;
    private float movementProgress = 0;

    private void Start() {
        controller = GetComponent<CharacterController>();
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        LockCursor();
    }

    private void FixedUpdate() {
        movementProgress += controller.velocity.sqrMagnitude * progressToStepRate * Time.fixedDeltaTime;
        if (movementProgress > 1) {
            AudioClip stepSound = stepSounds[stepSoundIndex];
            AudioSource.PlayClipAtPoint(stepSound, transform.position);
            movementProgress = 0;
            if (stepSoundIndex + 1 < stepSounds.Count) {
                stepSoundIndex++;
            }
            else stepSoundIndex = 0;
        }
    }

    private void Update()
    {
        InputManagement();
        Movement();
    }
    private void LateUpdate()
    {
        CameraBob();
    }

    private void Movement()
    {
        if(movementLocked) return;
        GroundMovement();
        Turn();
    }


    private void CameraBob() {        
        if(cameraBobLocked) return;
        bool isMoving = controller.isGrounded && controller.velocity.magnitude > 0.1f;
        float targetAmplitude = isMoving ? bobAmplitude * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeedMultiplier : 1f) : 0f;
        float targetFrequency = isMoving ? bobFrequency * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeedMultiplier : 1f) : 0f;

        currentBobAmplitude = Mathf.Lerp(currentBobAmplitude, targetAmplitude, Time.deltaTime * bobTransitionSpeed);
        currentBobFrequency = Mathf.Lerp(currentBobFrequency, targetFrequency, Time.deltaTime * bobTransitionSpeed);

        if (isMoving)
        {
            bobTimer += Time.deltaTime * currentBobFrequency;
            float bobbingEffect = Mathf.Sin(bobTimer) * currentBobAmplitude;

            noiseComponent.m_AmplitudeGain = Mathf.Abs(bobbingEffect);
        }
        else
        {
            noiseComponent.m_AmplitudeGain = currentBobAmplitude;
        }

        noiseComponent.m_FrequencyGain = currentBobFrequency;
    }



    private void GroundMovement()
    {
        Vector3 move = transform.forward * moveInput + transform.right * strafeInput;

        currentSpeed = CalculateCurrentSpeed();
        move *= currentSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }
    private void Turn()
    {
        float adjustedMouseX = mouseX * mouseSensitivity;
        float adjustedMouseY = mouseY * mouseSensitivity;

        xRotation -= adjustedMouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        virtualCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * adjustedMouseX);
    }
    private float CalculateCurrentSpeed()
    {
        float targetSpeed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeedMultiplier : 1f);
        return Mathf.Lerp(currentSpeed, targetSpeed, sprintTransitSpeed * Time.deltaTime);
    }
    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }
    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        strafeInput = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateMovementLock(bool locked) {
        movementLocked = locked;
        cameraBobLocked = locked;
        if (cameraBobLocked)
        {
            currentBobAmplitude = 0;
            currentBobFrequency = 0;
            noiseComponent.m_AmplitudeGain = 0;
            noiseComponent.m_FrequencyGain = 0;
        }
    }
}
