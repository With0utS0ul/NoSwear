using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cameraTransform;

    [Header("Attack References")]
    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private Transform magicFirePoint;
    [SerializeField] private GameObject magicProjectilePrefab;

    private Player player;
    private CharacterController controller;
    private float turnSmoothVelocity;
    private PlayerAnimator playerAnimator;

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
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacks();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
            playerAnimator?.SetSpeed(moveDir.magnitude);
        }
        else
        {
            playerAnimator?.SetSpeed(0);
        }
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