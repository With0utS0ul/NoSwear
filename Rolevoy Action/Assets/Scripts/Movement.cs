using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] private Canvas lose;
    
    public Characters_Stats.PlayerStats mk_Stats;

    [SerializeField] GameObject spear;
   

    [Header("Настройки магической атаки")]
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform firePoint; // Точка выстрела (перетащите в инспекторе)
    public float projectileSpeed = 20f;
    public float fireCooldown = 3f; // Перезарядка между выстрелами
    public float projectileDamage = 15f;

    private float lastFireTime = 0f;

    [Header("UI: Иконки магической атаки")]
    public UnityEngine.UI.Image iconMagicReady;    // Иконка "готова" (по умолчанию видна)
    public UnityEngine.UI.Image iconMagicCooldown;  // Иконка "кулдаун" (скрыта по умолчанию)
    public UnityEngine.UI.Image iconCooldownFill;   // Опционально: заполнение кулдауна (Mask или Slider)
    public Text cooldownText; // Опционально: текст с обратным отсчётом

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
        if (mk_Stats == null) mk_Stats = new Characters_Stats.PlayerStats();
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

        if (mk_Stats != null) mk_Stats.ResetStats();

        if (iconMagicReady != null) iconMagicReady.gameObject.SetActive(true);
        if (iconMagicCooldown != null) iconMagicCooldown.gameObject.SetActive(false);
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
            // Проверка перезарядки
            if (Time.time < lastFireTime + fireCooldown) return;

            lastFireTime = Time.time;
            PerformMagicAttack();

            // === ЛОГИКА СМЕНЫ ИКОНОК ===
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
                    projScript.Init(transform.forward,projectileSpeed);
                    projScript.speed = projectileSpeed;
                }
                projectile.transform.rotation = Quaternion.LookRotation(transform.forward);
            }
        }
    }

    private void StartMagicCooldownUI()
    {
        isMagicOnCooldown = true;

        // Переключаем иконки
        if (iconMagicReady != null) iconMagicReady.gameObject.SetActive(false);
        if (iconMagicCooldown != null) iconMagicCooldown.gameObject.SetActive(true);

        // Сбрасываем визуал заполнения, если есть
        if (iconCooldownFill != null)
            iconCooldownFill.fillAmount = 1f; // Полное заполнение в начале кулдауна
    }

    private void UpdateMagicCooldownUI()
    {
        if (!isMagicOnCooldown) return;

        float cooldownElapsed = Time.time - lastFireTime;
        float cooldownProgress = Mathf.Clamp01(cooldownElapsed / fireCooldown);

        // Обновляем заполнение (если используется Image с типом Filled)
        if (iconCooldownFill != null)
        {
            iconCooldownFill.fillAmount = 1f - cooldownProgress;
        }

        // Обновляем текст таймера (опционально)
        if (cooldownText != null)
        {
            float remaining = Mathf.Max(0f, fireCooldown - cooldownElapsed);
            cooldownText.text = remaining.ToString("F1"); // "2.3"
        }

        // Если кулдаун закончился — возвращаем иконку "готова"
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

    private void AttackCollider()
    {
        spear.GetComponent<Collider>().enabled = false;
    }

    private void PerformMeleeAttack()
    {
        spear.GetComponent<Collider>().enabled = true;
        Debug.Log("Удар мечом! Урон: " + mk_Stats.physicalDamage);
        animator.SetTrigger(attackMeleeHash);
        Invoke(nameof(AttackCollider), 1);
    }

    private void PerformMagicAttack()
    {
        Debug.Log("Магический удар! Урон: " + mk_Stats.magicDamage);
        animator.SetTrigger(attackMagicHash);
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isStunned) return;

        mk_Stats.currentHP -= damage;
        Debug.Log($"Получен урон: {damage}. Осталось HP: {mk_Stats.currentHP}");

        if (mk_Stats.currentHP <= 0)
        {
            Die();
            lose.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
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

    public void Die()
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

