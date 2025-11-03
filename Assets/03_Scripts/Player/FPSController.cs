using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public float interactRange = 3f;

    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    public float lookSensitivity = 1.5f;
    public float verticalLookLimit = 89f;

    private CharacterController controller;
    private PlayerControls controls;
    private IInteractable currentTarget;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float pitch = 0f;

    private float highlightCooldown = 0.05f;
    private float highlightTimer = 0f;

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        // Player Movement input
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        // Look input
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => lookInput = Vector2.zero;

        // Interact (mouse click or controller button)
        controls.Player.Interact.performed += _ => TryInteract();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        highlightTimer += Time.deltaTime;
        if (highlightTimer >= highlightCooldown)
        {
            CheckForInteractable();
            highlightTimer = 0f;
        }
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalLookLimit, verticalLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        bool grounded = controller.isGrounded;
        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void TryInteract()
    {
        if (currentTarget != null)
        {
            Debug.Log($"Interacting with {((MonoBehaviour)currentTarget).name}");
            currentTarget.Interact();
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // If we're looking at a new interactable
                if (interactable != currentTarget)
                {
                    // Disable previous highlight
                    if (currentTarget is IHighlightable prevHighlight)
                        prevHighlight.Highlight(false);

                    // Enable new highlight
                    currentTarget = interactable;
                    if (currentTarget is IHighlightable newHighlight)
                        newHighlight.Highlight(true);
                }
                return;
            }
        }

        // Nothing interactable in sight — clear highlight
        if (currentTarget is IHighlightable lastHighlight)
            lastHighlight.Highlight(false);

        currentTarget = null;
    }
}

// Interface to determine what Interactable objects must adhere to
public interface IInteractable
{
    void Interact();
}

public interface IHighlightable
{
    void Highlight(bool state);
}