using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -19.81f;
    public float airControl = 0.8f;
    public float pushForce = 2f;

    private CharacterController controller;
    private Vector3 verticalVelocity;
    private Vector3 horizontalVelocity;
    private bool isGrounded;
    private Animator animator;

    private InputAction movementAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction pauseAction;

    public Transform cameraTransform;
    public GameObject PickUp;
    public float lookSensitivity = 2f;
    private Vector2 lookInput;
    private float xRotation = 0f;

    public bool isPaused = false;
    public GameObject blackScreen;
    public GameObject win;

    [SerializeField]
    private InputActionAsset inputAsset;

    private void Awake()
    {
        Time.timeScale = 1f;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        movementAction = inputAsset.FindAction("Player/Move");
        lookAction = inputAsset.FindAction("Player/Look");
        jumpAction = inputAsset.FindAction("Player/Jump");
        interactAction = inputAsset.FindAction("Player/Interact");
        pauseAction = inputAsset.FindAction("Player/Pause");

        Cursor.lockState = CursorLockMode.Locked;

        blackScreen.SetActive(false);
        win.SetActive(false);
    }

    void OnEnable()
    {
        movementAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        interactAction.Enable();
        pauseAction.Enable();
    }
    void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        interactAction.Disable();
        pauseAction.Disable();
    }

    void Update()
    {
        if (pauseAction.triggered)
        {
            if(isPaused) Resume();
            else Pause();
        }

        if (isPaused)
            return;

        isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        Movement();
        Looking();
        Jumping();
        AnimatePlayer();

        if (interactAction.triggered)
        {
            Interact();
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 combiedVelcocity = horizontalVelocity + verticalVelocity;
        controller.Move(combiedVelcocity * Time.deltaTime);
    }
    void AnimatePlayer()
    {
        float currentSpeed = new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z).magnitude;
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("isGrounded", isGrounded);
    }
    void Movement()
    {
        Vector2 moveInput = movementAction.ReadValue<Vector2>();
        Vector3 inputDirection = (transform.right * moveInput.x + transform.forward * moveInput.y);
        if (isGrounded)
        {
            horizontalVelocity = inputDirection * moveSpeed;
        }
        else
        {
            Vector3 targetAirVelocity = inputDirection * moveSpeed;
            float acceleration = airControl * 50;
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetAirVelocity, acceleration * Time.deltaTime);
        }
    }

    void Looking()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float mouseX = lookInput.x * lookSensitivity * 0.1f;
        float mouseY = lookInput.y * lookSensitivity * 0.1f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jumping()
    {
        if (jumpAction.triggered && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            animator.SetTrigger("Jump");
        }
    }
    void Interact()
    {
        InteractionController interaction = PickUp.GetComponent<InteractionController>();

        interaction.Interact();
    }

    public void WinGame()
    {
        win.SetActive(true);
        Time.timeScale = 0f;
        OnDisable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        Vector3 pushDir = new(hit.moveDirection.x, 0, hit.moveDirection.z);

        rb.linearVelocity = pushDir * pushForce;

        rb.angularVelocity = Vector3.zero;
    }

    public void Resume()
    {
        isPaused = false;
        blackScreen.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        isPaused = true;
        blackScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
        void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("LavaFloor"))
        {
            blackScreen.SetActive(true);
            Time.timeScale = 0f;
            OnDisable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            WinGame();
        }
    }
}

