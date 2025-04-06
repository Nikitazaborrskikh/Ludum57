using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float doubleTapTime = 0.3f;
    [SerializeField] private float dashCooldown = 1f;  

    private PlayerControls playerControls;
    private Vector2 moveInput;
    private CharacterController controller;
    private bool isDashing = false;
    private float dashTimeLeft;
    private Vector3 dashDirection;
    private float dashCooldownTimer;                  
    
    private float lastTapTimeW = -1f;
    private float lastTapTimeA = -1f;
    private float lastTapTimeS = -1f;
    private float lastTapTimeD = -1f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        playerControls = new PlayerControls();
        playerControls.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Movement.Move.canceled += ctx => moveInput = Vector2.zero;
        
        dashCooldownTimer = 0f;                        
    }

    private void OnEnable()
    {
        playerControls.Movement.Enable();
    }

    private void OnDisable()
    {
        playerControls.Movement.Disable();
    }

    private void Update()
    {
        if (dashCooldownTimer > 0)                     
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            HandleMovement();
            CheckForDash();
        }
    }

    private void HandleMovement()
    {
        //Debug.Log("move");
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;
        
        if (!controller.isGrounded)
        {
            moveVelocity.y = -9.81f;
        }

        controller.Move(moveVelocity * Time.deltaTime);
    }

    private void HandleDash()
    {
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            isDashing = false;
            return;
        }

        controller.Move(dashDirection * (dashDistance / dashDuration) * Time.deltaTime);
    }

    private void CheckForDash()
    {
        if (dashCooldownTimer > 0) return;             

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeW <= doubleTapTime && moveInput.y > 0)
            {
                StartDash(Vector3.forward);
            }
            lastTapTimeW = Time.time;
        }
        
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeS <= doubleTapTime && moveInput.y < 0)
            {
                StartDash(Vector3.back);
            }
            lastTapTimeS = Time.time;
        }
        
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeA <= doubleTapTime && moveInput.x < 0)
            {
                StartDash(Vector3.left);
            }
            lastTapTimeA = Time.time;
        }
        
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeD <= doubleTapTime && moveInput.x > 0)
            {
                StartDash(Vector3.right);
            }
            lastTapTimeD = Time.time;
        }
    }

    private void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashDirection = direction.normalized;
        dashCooldownTimer = dashCooldown;               
    }

    public float GetDashCooldownRemaining()
    {
        return Mathf.Max(0, dashCooldownTimer);
    }

    public float GetDashCooldownProgress()
    {
        return 1 - (dashCooldownTimer / dashCooldown);
    }
}