using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerControls.IMovementActions
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float doubleTapTime = 0.3f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject primaryBulletPrefab;
    [SerializeField] private GameObject secondaryBulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationDeadZone = 0.1f;

    private PlayerControls playerControls;
    private Vector2 moveInput;
    private CharacterController controller;

    // Переменные для даша
    private bool isDashing = false;
    private float dashTimeLeft;
    private Vector3 dashDirection;
    private float dashCooldownTimer;

    // Переменные для отслеживания двойного нажатия
    private float lastTapTimeW = -1f;
    private float lastTapTimeA = -1f;
    private float lastTapTimeS = -1f;
    private float lastTapTimeD = -1f;

    private Vector3 lastTargetPoint;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        playerControls = new PlayerControls();
        playerControls.Movement.SetCallbacks(this);
        dashCooldownTimer = 0f;
        lastTargetPoint = transform.position + transform.forward;
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

        RotateTowardsCursor();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot(primaryBulletPrefab);
        }
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot(secondaryBulletPrefab);
        }
    }

    private void RotateTowardsCursor()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            targetPoint.y = transform.position.y;

            if (Vector3.Distance(targetPoint, lastTargetPoint) > rotationDeadZone)
            {
                Vector3 direction = (targetPoint - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                lastTargetPoint = targetPoint;
            }
        }
    }

    private void Shoot(GameObject bulletPrefab)
    {
        // Получаем позицию курсора в экранных координатах
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position); // Плоскость на уровне персонажа
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            // Получаем точку в мире, куда указывает курсор
            Vector3 targetPoint = ray.GetPoint(distance);
            targetPoint.y = bulletSpawnPoint.position.y; // Фиксируем Y на уровне точки спавна

            // Вычисляем направление от точки спавна к курсору
            Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;

            // Создаем пулю и задаем ей скорость
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;

                // (Опционально) Поворачиваем пулю в направлении движения
                bullet.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            else
            {
                Debug.LogWarning("Bullet prefab is missing a Rigidbody component!");
            }
        }
        else
        {
            Debug.LogWarning("Raycast failed to hit the plane!");
        }
    }

    private void HandleMovement()
    {
        //Debug.Log("Move");
        Vector3 moveDirection = Vector3.zero;
        moveDirection += transform.forward * moveInput.y;
        moveDirection += transform.right * moveInput.x;
        moveDirection = moveDirection.normalized;
        
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
                StartDash(transform.forward);
            }
            lastTapTimeW = Time.time;
        }
        
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeS <= doubleTapTime && moveInput.y < 0)
            {
                StartDash(-transform.forward);
            }
            lastTapTimeS = Time.time;
        }
        
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeA <= doubleTapTime && moveInput.x < 0)
            {
                StartDash(-transform.right);
            }
            lastTapTimeA = Time.time;
        }
        
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeD <= doubleTapTime && moveInput.x > 0)
            {
                StartDash(transform.right);
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