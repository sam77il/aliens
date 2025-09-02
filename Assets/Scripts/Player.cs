using UnityEngine;

public class Player : MonoBehaviour
{
    private InputSystem_Actions inputActions;
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

    private Vector3 currentMovement;
    private float verticalRotation;
    private float CurrentSpeed => walkSpeed * (SprintTriggered ? sprintMultiplier : 1.0f);

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }


    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => MovementInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => RotationInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => RotationInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx =>
        {
            JumpTriggered = true;
            GameDebug.Instance.UpdateDebugText("jumping", JumpTriggered);
        };
        inputActions.Player.Jump.canceled += ctx =>
        {
            JumpTriggered = false;
            GameDebug.Instance.UpdateDebugText("jumping", JumpTriggered);
        };

        inputActions.Player.Sprint.performed += ctx => SprintTriggered = true;
        inputActions.Player.Sprint.canceled += ctx => SprintTriggered = false;
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

            if (JumpTriggered)
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

    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
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

        ApplyHorizontalRotation(mouseX);
        ApplyVerticalRotation(mouseY);
    }
}
