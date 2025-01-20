using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    private Transform cameraTransform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

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

    [Header("Input")]
    [SerializeField] private float mouseSensitivity = 100f;
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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cameraTransform = virtualCamera.transform;
        LockCursor();
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
        GroundMovement();
        Turn();
    }


    private void CameraBob()
    {
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
        Vector3 move = new Vector3(strafeInput, 0, moveInput);
        move = cameraTransform.TransformDirection(move);

        currentSpeed = CalculateCurrentSpeed();
        move *= currentSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }
    private void Turn()
    {
        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        virtualCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
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
}
