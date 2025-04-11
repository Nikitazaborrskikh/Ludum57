using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;
using static UnityEngine.Rendering.DebugUI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, PlayerControls.IMovementActions, IPausable
{
    public AudioClip dashSound;
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip dieSound;
    public GameObject audioSource;

    public GameObject managers;

    [Header("Movement Settings")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float doubleTapTime = 0.3f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Rotation Settings")]
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float rotationSpeedAngle = 200f;
    [SerializeField] private float rotationDeadZone = 0.1f;
    private PlayerStats playerStats;
    [Inject]
    public void Construct(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        Debug.Log("PlayerStats injected into PlayerController");
    }
    [SerializeField] private PlayerShooting playerShooting;

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

    private Vector3 lastTargetPoint;
    private bool isPaused;
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Movement.SetCallbacks(this);
        if (playerStats == null)
            Debug.LogError("playerStats is null in Awake!", this);
        else
            Debug.Log("playerStats injected successfully in Awake", this);
    }

    private void Start()
    {
        playerStats.damageSound = damageSound;
        playerStats.dieSound = dieSound;
        playerStats.audioSource = audioSource.GetComponent<AudioSource>();
        playerStats.managers = managers;
        controller = GetComponent<CharacterController>();
        if (playerShooting == null)
            playerShooting = GetComponent<PlayerShooting>();
        dashCooldownTimer = 0f;
        lastTargetPoint = transform.position + transform.forward;
    }

    private void OnEnable()
    {
        playerControls.Movement.Enable(); // Теперь playerControls точно не null
    }
    private void OnDisable()
    {
        playerControls.Movement.Disable();
    }

    public void Pause()
    {
        isPaused = true;
        playerControls.Disable();
    }
    public void Resume()
    {
        isPaused = false;
        playerControls.Enable();
    }
    private void Update()
    {
        if (isPaused) return;
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not injected yet!", this);
            return;
        }

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (isDashing)
            HandleDash();
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
        playerShooting.OnPrimaryAttack(context, shootSound, audioSource.GetComponent<AudioSource>());
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        playerShooting.OnSecondaryAttack(context, shootSound, audioSource.GetComponent<AudioSource>());
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

            // Ограничиваем расстояние от игрока до targetPoint
            Vector3 directionToTarget = (targetPoint - transform.position);
            if (directionToTarget.magnitude > maxDistance)
            {
                targetPoint = transform.position + directionToTarget.normalized * maxDistance;
            }

            // Проверяем мёртвую зону
            if (Vector3.Distance(targetPoint, lastTargetPoint) > rotationDeadZone)
            {
                Vector3 direction = (targetPoint - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeedAngle * Time.deltaTime);

                lastTargetPoint = targetPoint; // Обновляем последнюю точку
            }
        }
    }
    // private void RotateTowardsCursor()
    // {
    //     Vector2 mousePosition = Mouse.current.position.ReadValue();
    //     Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    //     Plane plane = new Plane(Vector3.up, transform.position);
    //     float distance;
    //
    //     if (plane.Raycast(ray, out distance))
    //     {
    //         Vector3 targetPoint = ray.GetPoint(distance);
    //         targetPoint.y = transform.position.y;
    //
    //         if (Vector3.Distance(targetPoint, lastTargetPoint) > rotationDeadZone)
    //         {
    //             Vector3 direction = (targetPoint - transform.position).normalized;
    //             Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
    //             transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeedAngle * Time.deltaTime);
    //             lastTargetPoint = targetPoint;
    //         }
    //     }
    // }

    private void HandleMovement()
    {

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is null!", this);
            return;
        }

        Vector3 moveDirection = Vector3.zero;
        moveDirection += transform.forward * moveInput.y;
        moveDirection += transform.right * moveInput.x; // Исправил опечатку: "direction" -> "right"
        moveDirection = moveDirection.normalized;
        //  Debug.Log($"moveInput: {moveInput}, moveDirection: {moveDirection}", this);
        Vector3 moveVelocity = moveDirection * playerStats.MoveSpeed;

        if (!controller.isGrounded)
            moveVelocity.y = -9.81f;

        controller.Move(moveVelocity * Time.deltaTime);
    }
    private IEnumerator StartSound(AudioClip Sound)
    {
        audioSource.GetComponent<AudioSource>().PlayOneShot(Sound);
        yield return null;
    }

    private void HandleDash()
    {
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            isDashing = false;
            return;
        }

        StartCoroutine(StartSound(dashSound));
        controller.Move(dashDirection * (playerStats.DashDistance / dashDuration) * Time.deltaTime);
    }

    private void CheckForDash()
    {
        if (dashCooldownTimer > 0) return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeW <= doubleTapTime && moveInput.y > 0)
                StartDash(transform.forward);
            lastTapTimeW = Time.time;
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeS <= doubleTapTime && moveInput.y < 0)
                StartDash(-transform.forward);
            lastTapTimeS = Time.time;
        }

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeA <= doubleTapTime && moveInput.x < 0)
                StartDash(-transform.right);
            lastTapTimeA = Time.time;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (Time.time - lastTapTimeD <= doubleTapTime && moveInput.x > 0)
                StartDash(transform.right);
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