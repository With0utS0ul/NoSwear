using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [Header("Характеристики")]
    public PlayerStats stats;

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
    public float animSmoothSpeed = 10f; // Сглаживание перехода между анимациями

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;

    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isDead = false;
    private bool isStunned = false;
    private float stunTimer = 0f;

    // Переменные для плавности анимации
    private float currentAnimSpeed = 0f;
    private float targetAnimSpeed = 0f;

    // Хэш параметров
    private int speedHash;
    private int attackMeleeHash;
    private int attackMagicHash;
    private int hitHash;
    private int deathHash;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // Инициализация хэшей
        speedHash = Animator.StringToHash("Speed");
        attackMeleeHash = Animator.StringToHash("AttackMelee");
        attackMagicHash = Animator.StringToHash("AttackMagic");
        hitHash = Animator.StringToHash("Hit");
        deathHash = Animator.StringToHash("Death");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (stats != null) stats.ResetStats();
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Направление движения
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Гравитация
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);

        // --- ЛОГИКА ДЛЯ АНИМАЦИИ ---
        // Вычисляем целевое значение скорости для аниматора
        // 0 = Стоим, 0.5 = Идем, 1 = Бежим
        float inputMagnitude = new Vector2(x, z).magnitude;

        if (inputMagnitude > 0.1f) // Если есть ввод
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
            PerformMagicAttack();
        }
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Удар мечом! Урон: " + stats.physicalDamage);
        animator.SetTrigger(attackMeleeHash);
    }

    private void PerformMagicAttack()
    {
        Debug.Log("Магический удар! Урон: " + stats.magicDamage);
        animator.SetTrigger(attackMagicHash);
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isStunned) return;

        stats.currentHP -= damage;
        Debug.Log($"Получен урон: {damage}. Осталось HP: {stats.currentHP}");

        if (stats.currentHP <= 0)
        {
            Die();
        }
        else
        {
            GetHit();
        }
    }

    private void GetHit()
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

    private void Die()
    {
        isDead = true;
        animator.SetTrigger(deathHash);
        controller.enabled = false;
    }

    private void UpdateAnimator()
    {
        // Плавное изменение параметра Speed (Lerp)
        currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetAnimSpeed, Time.deltaTime * animSmoothSpeed);
        animator.SetFloat(speedHash, currentAnimSpeed);
    }
}

[System.Serializable]
public class PlayerStats
{
    public float maxHP = 100f;
    [HideInInspector] public float currentHP = 100f;
    public float physicalDamage = 10f;
    public float magicDamage = 25f;

    public void ResetStats()
    {
        currentHP = maxHP;
    }
}