using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComp))]
public class Movement : MonoBehaviour
{
    [SerializeField] private Canvas lose;

    public PlayerStats mk_Stats { get; private set; }

    private HealthComp health;
    [Header("Настройки магической атаки")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public float fireCooldown = 3f;
    public float projectileDamage = 15f;

    private float lastFireTime = 0f;

    [Header("UI: Иконки магической атаки")]
    public UnityEngine.UI.Image iconMagicReady;
    public UnityEngine.UI.Image iconMagicCooldown;
    public Text cooldownText;

    private bool isMagicOnCooldown = false;


    [Header("Настройки движения")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    [Header("Настройки камеры")]
    public Transform cameraPivot;
    public float mouseSensitivity = 2f;
    public float minCameraAngle = -45f;
    public float maxCameraAngle = 85f;

    [Header("Настройки анимации")]
    public float animSmoothSpeed = 10f;

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;

    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isDead = false;
    private bool isStunned = false;
    private float stunTimer = 0f;

    private float currentAnimSpeed = 0f;
    private float targetAnimSpeed = 0f;

    private int speedHash;
    private int attackMeleeHash;
    private int attackMagicHash;
    private int hitHash;
    private int deathHash;

    private void Awake()
    {
        health = GetComponent<HealthComp>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        speedHash = Animator.StringToHash("Speed");
        attackMeleeHash = Animator.StringToHash("AttackMelee");
        attackMagicHash = Animator.StringToHash("AttackMagic");
        hitHash = Animator.StringToHash("Hit");
        deathHash = Animator.StringToHash("Death");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mk_Stats == null) mk_Stats = new PlayerStats();
        mk_Stats.Initialize(health);
        if (mk_Stats != null) mk_Stats.ResetStats();
        mk_Stats.OnDeath += HandleDeath;



        if (iconMagicReady != null) iconMagicReady.gameObject.SetActive(true);
        if (iconMagicCooldown != null) iconMagicCooldown.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (mk_Stats != null)
        {
            mk_Stats.OnDeath -= HandleDeath;
        }
    }
    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }

    private void Update()
    {
        if (isDead) return;

        HandleStun();
        HandleCameraRotation();

        if (!isStunned)
        {
            HandleMovement();
            HandleCombat();
        }

        UpdateAnimator();
        UpdateMagicCooldownUI();
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minCameraAngle, maxCameraAngle);

        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        if (!controller.enabled) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);

        float inputMagnitude = new Vector2(x, z).magnitude;

        if (inputMagnitude > 0.1f)
        {
            targetAnimSpeed = isRunning ? 1f : 0.5f;
        }
        else
        {
            targetAnimSpeed = 0f;
        }
    }

    private void HandleCombat()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            PerformMeleeAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time < lastFireTime + fireCooldown) return;

            lastFireTime = Time.time;
            PerformMagicAttack();
            StartMagicCooldownUI();

            if (projectilePrefab != null)
            {
                Vector3 spawnPosition = firePoint != null ?
                    firePoint.position :
                    transform.position + transform.forward * 1.5f + Vector3.up * 1f;

                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                Projectile projScript = projectile.GetComponent<Projectile>();
                if (projScript != null)
                {
                    projScript.Init(transform.forward);
                    projScript.speed = projectileSpeed;
                }
                projectile.transform.rotation = Quaternion.LookRotation(transform.forward);
            }
        }
    }

    private void StartMagicCooldownUI()
    {
        isMagicOnCooldown = true;
        if (iconMagicReady != null) iconMagicReady.gameObject.SetActive(false);
        if (iconMagicCooldown != null) iconMagicCooldown.gameObject.SetActive(true);
    }

    private void UpdateMagicCooldownUI()
    {
        if (!isMagicOnCooldown) return;

        float cooldownElapsed = Time.time - lastFireTime;
        float cooldownProgress = Mathf.Clamp01(cooldownElapsed / fireCooldown);

        if (cooldownText != null)
        {
            float remaining = Mathf.Max(0f, fireCooldown - cooldownElapsed);
            cooldownText.text = remaining.ToString("F1");
        }
        if (cooldownElapsed >= fireCooldown)
        {
            EndMagicCooldownUI();
        }
    }

    private void EndMagicCooldownUI()
    {
        isMagicOnCooldown = false;

        if (iconMagicReady != null) iconMagicReady.gameObject.SetActive(true);
        if (iconMagicCooldown != null) iconMagicCooldown.gameObject.SetActive(false);

        if (cooldownText != null) cooldownText.text = "";
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Удар мечом! Урон: " + mk_Stats.physicalDamage);
        animator.SetTrigger(attackMeleeHash);
    }

    private void PerformMagicAttack()
    {
        Debug.Log("Магический удар! Урон: " + mk_Stats.magicDamage);
        animator.SetTrigger(attackMagicHash);
    }

    public void TakeDamage(float damage)
    {
        mk_Stats.TakeDamage(damage);
        if (mk_Stats.IsAlive)
        {
            GetHit();
        }
    }

    public void GetHit()
    {
        isStunned = true;
        stunTimer = 0.5f;
        animator.SetTrigger(hitHash);
    }

    private void HandleStun()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) isStunned = false;
        }
    }
    private void HandleDeath()
    {
        if (isDead) return;
        enabled = false;
        isDead = true;
        animator.SetTrigger(deathHash);
        controller.enabled = false;

        lose?.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger(deathHash);
        controller.enabled = false;
    }

    private void UpdateAnimator()
    {
        currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetAnimSpeed, Time.deltaTime * animSmoothSpeed);
        animator.SetFloat(speedHash, currentAnimSpeed);
    }
}

