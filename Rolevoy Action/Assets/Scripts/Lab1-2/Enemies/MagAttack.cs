using UnityEngine;

public class MagAttack : MonoBehaviour
{
    [SerializeField] private float coolDown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    // Эти параметры можно менять через профиль (цвет, скорость, но не урон)
    private float currentProjectileSpeed = 20f;
    private Color currentProjectileColor = Color.white;

    public bool CanAttack { get; private set; } = true;
    private float lastAttackTime;

    private EnemyView enemyView; // получаем урон оттуда

    public float CoolDown => coolDown;

    private void Start()
    {
        enemyView = GetComponent<EnemyView>();
        if (enemyView == null)
            enemyView = GetComponentInParent<EnemyView>();
    }

    private void Update()
    {
        if (Time.time - lastAttackTime >= coolDown)
            CanAttack = true;
    }

    public void TryAttackPlayer(Transform playerTransform)
    {
        if (!CanAttack) return;
        if (projectilePrefab != null && firePoint != null && enemyView != null)
        {
            GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector3 direction = (playerTransform.position - firePoint.position).normalized;
                // Урон берём из EnemyView (через Enemy.EnemyAttack или напрямую)
                float damage = GetDamageFromEnemyView();
                projectile.Init(direction, currentProjectileSpeed, DamageType.Magical, damage);
                projectileObj.tag = "EnemyProjectile";

                // Цвет снаряда (только визуал)
                Renderer rend = projectileObj.GetComponent<Renderer>();
                if (rend != null)
                    rend.material.color = currentProjectileColor;
            }
        }
        CanAttack = false;
        lastAttackTime = Time.time;
    }
    public void ApplyBossProfile(BossWeaponProfile profile)
    {
        if (profile == null) return;
        currentProjectileSpeed = profile.projectileSpeed;
        currentProjectileColor = profile.projectileColor;
        if (profile.projectilePrefab != null)
            projectilePrefab = profile.projectilePrefab;
    }
    private float GetDamageFromEnemyView()
    {
        if (enemyView == null) return 10f;
        // Вариант 1: если у EnemyView есть публичное поле rangedDamage
        // (в вашем EnemyView rangedDamage есть в инспекторе, но оно private, нужно сделать public или добавить свойство)
        // return enemyView.RangedDamage; 

        // Вариант 2: через компонент MeleeWeapon или RangedAttack, но проще сделать свойство
        // Пока вернём дефолтное значение, вы потом добавите геттер
        return 10f;
    }

    public void ApplyProfile(AttackProfile profile)
    {
        if (profile == null) return;

        // Меняем только то, что не хранится в EnemyView (цвет, скорость, префаб, кулдаун)
        currentProjectileSpeed = profile.projectileSpeed;
        currentProjectileColor = profile.projectileColor;

        if (profile.projectilePrefab != null)
            projectilePrefab = profile.projectilePrefab;

        if (profile.cooldown > 0)
            coolDown = profile.cooldown;

        // Урон не трогаем — он берётся из EnemyView
    }
}