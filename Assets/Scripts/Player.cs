using UnityEngine;

public class Player : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Move(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime * speed);
        Debug.Log("Player moved: " + direction);
    }

    private void Start()
    {
        Debug.Log("Player script has started.");
    }
}
