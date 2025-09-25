using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMultiplier = 2f;
    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 1f;
    [Header("Look Speeds")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;

    private InputSystem_Actions inputActions;
    private Vector3 currentMovement;
    private float verticalRotation;
    private float CurrentSpeed => walkSpeed * (IsSprinting ? sprintMultiplier : 1.0f);

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsCrouching { get; private set; }

    private Item isInteractable;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx =>
        {
            MovementInput = ctx.ReadValue<Vector2>();
            GameDebug.Instance.UpdateDebugText("walking", true);
        };
        inputActions.Player.Move.canceled += ctx =>
        {
            MovementInput = Vector2.zero;
            GameDebug.Instance.UpdateDebugText("walking", false);
        };

        inputActions.Player.Crouch.performed += ctx =>
        {
            characterController.height = 0.5f;
            IsCrouching = true;
            GameDebug.Instance.UpdateDebugText("crouching", IsCrouching);
        };
        inputActions.Player.Crouch.canceled += ctx =>
        {
            characterController.height = 1f;
            IsCrouching = false;
            GameDebug.Instance.UpdateDebugText("crouching", IsCrouching);
        };

        inputActions.Player.Look.performed += ctx => RotationInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => RotationInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx =>
        {
            IsJumping = true;
            GameDebug.Instance.UpdateDebugText("jumping", IsJumping);
        };
        inputActions.Player.Jump.canceled += ctx =>
        {
            IsJumping = false;
            GameDebug.Instance.UpdateDebugText("jumping", IsJumping);
        };

        inputActions.Player.Sprint.performed += ctx =>
        {
            IsSprinting = true;
            GameDebug.Instance.UpdateDebugText("sprinting", IsSprinting);
        };
        inputActions.Player.Sprint.canceled += ctx =>
        {
            IsSprinting = false;
            GameDebug.Instance.UpdateDebugText("sprinting", IsSprinting);
        };
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }


    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 2f);
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 2f, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            isInteractable = hit.collider.CompareTag("Interactable") ? hit.collider.GetComponent<Item>() : null;
        }
        else
        {
            isInteractable = null;
        }
    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(inputActions.Player.Move.ReadValue<Vector2>().x, 0f, inputActions.Player.Move.ReadValue<Vector2>().y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    private void HandleJumping()
    {
        GameDebug.Instance.UpdateDebugText("grounded", characterController.isGrounded);
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (IsJumping)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleRotation()
    {
        float mouseX = inputActions.Player.Look.ReadValue<Vector2>().x * mouseSensitivity;
        float mouseY = inputActions.Player.Look.ReadValue<Vector2>().y * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);
        ApplyVerticalRotation(mouseY);
    }
}
