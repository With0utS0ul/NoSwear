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

        // ¬оспроизведение звука
        if (currentWeapon.attackSound != null)
            AudioSource.PlayClipAtPoint(currentWeapon.attackSound, context.transform.position);

        // Ёффект дульного всплеска дл€ дальнего оружи€
        if (!currentWeapon.isMelee && currentWeapon.muzzleFlashPrefab != null)
            Instantiate(currentWeapon.muzzleFlashPrefab, context.transform.position, Quaternion.identity);

        // Ќанесение урона (пример Ц через EnemyView)
        if (target != null && context.enemyView != null && context.enemyView.Enemy != null)
        {
            // «десь вызываетс€ существующа€ система атаки врага
            context.enemyView.Enemy.Attack();
        }

        // Ёффект при попадании будет заспавнен в другом месте (например, в классе здоровь€ игрока)
    }

    // ƒл€ проверки типа оружи€ в ChaseState
    public bool HasRangedWeapon => !currentWeapon.isMelee;
    public float GetOptimalDistance => currentWeapon.optimalDistance;
    public float GetAttackRange => currentWeapon.range;
}