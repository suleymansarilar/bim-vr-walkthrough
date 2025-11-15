using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;
    public float upDownRange = 60f;

    private CharacterController controller;
    private Camera playerCamera;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
       
        HandleMouseLook();

        
        HandleMovement();

        
        // ESC key: Toggle cursor lock/unlock (only if not resetting task)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

       
        transform.Rotate(0, mouseX, 0);

       
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -upDownRange, upDownRange);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void HandleMovement()
    {
        if (controller.isGrounded)
        {
            // Use WASD keys directly instead of Input Axis
            float horizontal = 0f;
            float vertical = 0f;

            // A/D keys for horizontal movement (Left/Right)
            if (Input.GetKey(KeyCode.A))
                horizontal = -1f;
            else if (Input.GetKey(KeyCode.D))
                horizontal = 1f;

            // W/S keys for vertical movement (Forward/Backward)
            if (Input.GetKey(KeyCode.W))
                vertical = 1f;
            else if (Input.GetKey(KeyCode.S))
                vertical = -1f;

            // Normalize movement vector for consistent speed
            Vector2 moveInput = new Vector2(horizontal, vertical);
            if (moveInput.magnitude > 1f)
            {
                moveInput.Normalize();
                horizontal = moveInput.x;
                vertical = moveInput.y;
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Running with Left Shift
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            moveDirection = (forward * vertical + right * horizontal) * currentSpeed;

            // Jump with Space (Space is now used for guided tour, so jump only when not in tour)
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
    }
}