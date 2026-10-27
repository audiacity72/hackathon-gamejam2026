using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class SmoothCameraMouseLook : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Array of targets the camera can cycle through and focus on.")]
    public Transform[] targets;

    [Tooltip("Index of the currently active target.")]
    public int currentIndex = 0;

    [Header("Position Settings")]
    [Tooltip("Base offset applied from the first target to establish the fixed camera position.")]
    public Vector3 baseOffset = new Vector3(0f, 5f, -10f);

    [Header("Rotation & Look Settings")]
    [Tooltip("Total horizontal angle range in degrees (e.g., 180 means 90 left, 90 right).")]
    public float maxHorizontalAngle = 180f;

    [Tooltip("Total vertical angle range in degrees (e.g., 130 means 65 up, 65 down).")]
    public float maxVerticalAngle = 130f;

    [Tooltip("How much the mouse position influences the look offset.")]
    public float mouseSensitivity = 1f;

    [Header("Smoothing Settings")]
    [Tooltip("Smoothing factor for camera rotation. Higher values mean faster movement.")]
    public float smoothSpeed = 10f;

    [Header("Drag Modifier Settings")]
    [Tooltip("Multiplier applied to mouse sensitivity while a keyword card is being dragged. Lower values mean slower camera tracking.")]
    [Range(0f, 1f)]
    public float dragSensitivityMultiplier = 0.2f;

    [Header("Zoom Settings (Z Key)")]
    [Tooltip("Default Field of View of the camera.")]
    public float normalFOV = 60f;

    [Tooltip("Zoomed-in Field of View when holding the Z key.")]
    public float zoomedFOV = 30f;

    [Tooltip("Speed at which the camera zooms in and out.")]
    public float zoomSpeed = 10f;

    private Camera cam;
    // Stores the locked world coordinate established at the first target
    private Vector3 lockedCameraPosition;

    void Start()
    {
        // Cache the camera component
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (targets != null && targets.Length > 0 && targets[0] != null)
        {
            // Track and lock the coordinates of the camera based on the first target
            lockedCameraPosition = targets[0].position + baseOffset;
            transform.position = lockedCameraPosition;
        }
        else
        {
            // Fallback if no targets are assigned yet
            lockedCameraPosition = transform.position;
        }
    }

    void LateUpdate()
    {
        if (targets == null || targets.Length == 0) return;

        // Handle target cycling with A and D keys using the new Input System
        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                currentIndex = (currentIndex + 1) % targets.Length; // Cycle forward
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                currentIndex = (currentIndex - 1 + targets.Length) % targets.Length; // Cycle backward
            }
        }

        // 1. Maintain the exact camera position coordinate established at the first target
        transform.position = lockedCameraPosition;

        // 2. Handle Zoom with Z key (Hold Z to zoom in)
        if (cam != null && Keyboard.current != null)
        {
            float targetFOV = Keyboard.current.zKey.isPressed ? zoomedFOV : normalFOV;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
        }

        // Ensure mouse input is active
        if (Mouse.current == null) return;

        // 3. Get mouse position using the new Input System (-0.5 to 0.5 from center)
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float mouseX = (mousePos.x / Screen.width) - 0.5f;
        float mouseY = (mousePos.y / Screen.height) - 0.5f;

        // 4. Adjust sensitivity if dragging a card
        float activeSensitivity = mouseSensitivity;
        if (DraggableKeywordCard.isDraggingAnyCard)
        {
            activeSensitivity *= dragSensitivityMultiplier;
        }

        // 5. Calculate yaw (horizontal) and pitch (vertical) offsets in degrees
        float yawOffset = mouseX * activeSensitivity * maxHorizontalAngle;
        
        // Pitch is inverted because pushing the mouse up (positive Y) should rotate the camera up (negative Pitch in Unity)
        float pitchOffset = -mouseY * activeSensitivity * maxVerticalAngle; 

        // 6. Clamp the angles to strictly enforce the requested limits
        yawOffset = Mathf.Clamp(yawOffset, -maxHorizontalAngle / 2f, maxHorizontalAngle / 2f);
        pitchOffset = Mathf.Clamp(pitchOffset, -maxVerticalAngle / 2f, maxVerticalAngle / 2f);

        // 7. Get the base rotation looking directly at the target
        Transform currentTarget = targets[currentIndex];
        if (currentTarget == null) return;

        Vector3 directionToTarget = currentTarget.position - transform.position;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion baseRotation = Quaternion.LookRotation(directionToTarget);

            // 8. Apply the clamped local angular offsets
            Quaternion offsetRotation = Quaternion.Euler(pitchOffset, yawOffset, 0f);
            Quaternion targetRotation = baseRotation * offsetRotation;

            // 9. Smoothly rotate the camera
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }
}