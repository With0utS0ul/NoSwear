// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Attack References")]
    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private Transform magicFirePoint;
    [SerializeField] private GameObject magicProjectilePrefab;

    private Player player;
    private CharacterController controller;
    private PlayerAnimator playerAnimator;
    private Vector3 velocity;

    public void Init(Player player)
    {
        this.player = player;
        player.SetMeleeWeapon(meleeWeapon);
        player.SetMagicAttack(magicFirePoint, magicProjectilePrefab);

        playerAnimator = GetComponent<PlayerAnimator>();
        player.OnDeath += () => playerAnimator?.SetDeath();
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacks();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (moveDirection.magnitude >= 0.1f)
        {
            controller.Move(moveDirection * speed * Time.deltaTime);
            playerAnimator?.SetSpeed(moveDirection.magnitude);
        }
        else
        {
            playerAnimator?.SetSpeed(0);
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleAttacks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.AttackMelee();
            playerAnimator?.SetAttackMelee();
        }
        if (Input.GetMouseButtonDown(1))
        {
            player.AttackMagic();
            playerAnimator?.SetAttackMagic();
        }
    }
}