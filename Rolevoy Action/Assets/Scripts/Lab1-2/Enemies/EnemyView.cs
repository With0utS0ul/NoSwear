using System;
using System.Runtime.InteropServices;
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
    [SerializeField] private float hitReactionCooldown = 0.25f;
    [SerializeField] private float speed = 1f;

    public event Action<EnemyView> OnDied;

    private Animator animator;
    private float lastHitReactionTime = -999f;

    public float RangedDamage => rangedDamage;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        IHealth health = new Health(maxHealth);

        IAttack attack = null;



        health.OnDamage += () =>
        {
            if (Enemy != null)
            {
                if (animator != null && Time.time >= lastHitReactionTime + hitReactionCooldown)
                {
                    animator.SetTrigger("GetDamage");
                    lastHitReactionTime = Time.time;
                }
            }


        };

        health.OnDeath += () =>
        {
            if (Enemy != null)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Death");
                    
                }
                bool isBoss = GetComponent<BossTag>() != null;
                OnDied?.Invoke(this);

                Destroy(gameObject, 1.0f);
            }
        };
       
        if (useMelee && meleeWeapon != null)
            attack = new MeleeAttack(meleeWeapon);

        if (useRanged && firePoint != null && projectilePrefab != null)
            attack = new RangedAttack(firePoint, projectilePrefab, rangedDamage, speed);

        Enemy = new Enemy(health, attack);
    }

}