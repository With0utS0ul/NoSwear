using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public Enemy Enemy { get; private set; }

    [Header("Config")]
    [SerializeField] private float maxHealth = 50f;

    [Header("Attack Settings")]
    [SerializeField] private bool useMelee;
    [SerializeField] private MeleeWeapon meleeWeapon;

    [SerializeField] private bool useRanged;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float rangedDamage = 10f;

    private void Awake()
    {
        IHealth health = new Health(maxHealth);

        IAttack attack = null;

        health.OnDeath += () =>
        {
            if (Enemy != null)
            {
                Animator animator = GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Death");
                }
                Destroy(gameObject, 2f);
            }
        };
        if (useMelee && meleeWeapon != null)
            attack = new MeleeAttack(meleeWeapon);

        if (useRanged && firePoint != null && projectilePrefab != null)
            attack = new RangedAttack(firePoint, projectilePrefab, rangedDamage);

        Enemy = new Enemy(health, attack);
    }

}