using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;
    private float lastAttackTime;

    public WeaponData CurrentWeapon => currentWeapon;

    public void SetWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;
    }

    public bool CanAttack => Time.time >= lastAttackTime + currentWeapon.cooldown;

    public void PerformAttack(EnemyContext context, Transform target)
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;

        // Воспроизведение звука
        if (currentWeapon.attackSound != null)
            AudioSource.PlayClipAtPoint(currentWeapon.attackSound, context.transform.position);

        // Эффект дульного всплеска для дальнего оружия
        if (!currentWeapon.isMelee && currentWeapon.muzzleFlashPrefab != null)
            Instantiate(currentWeapon.muzzleFlashPrefab, context.transform.position, Quaternion.identity);

        // Нанесение урона (пример – через EnemyView)
        if (target != null && context.enemyView != null && context.enemyView.Enemy != null)
        {
            // Здесь вызывается существующая система атаки врага
            context.enemyView.Enemy.Attack();
        }

        
    }

    // Для проверки типа оружия в ChaseState
    public bool HasRangedWeapon => !currentWeapon.isMelee;
    public float GetOptimalDistance => currentWeapon.optimalDistance;
    public float GetAttackRange => currentWeapon.range;
} 