using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField] private AttackProfile currentProfile;
    private float lastAttackTime;

    private readonly EnemyContext context;
    public AttackProfile CurrentProfile => currentProfile;

    public void SetProfile(AttackProfile profile)
    {
        currentProfile = profile;
    }

    public bool CanAttack => Time.time >= lastAttackTime + currentProfile.cooldown;

    public void PerformAttack(EnemyContext context, Transform target)
    {
        if (!CanAttack) return;

        if (CurrentProfile.isMelee)
        {
            if (context.meleeAttack != null)
                context.meleeAttack.TryAttackPlayer();
            else
                context.enemyView?.Enemy?.Attack(); // fallback
        }
        else
        {
            if (context.rangedAttack != null)
                context.rangedAttack.TryAttackPlayer(target);
            else
                Debug.LogWarning("Ranged attack component missing");
        }

        lastAttackTime = Time.time;

        // «вук
        if (currentProfile.attackSound != null)
            AudioSource.PlayClipAtPoint(currentProfile.attackSound, context.transform.position);

        // ¬изуальный эффект атаки (укус, вспышка магии)
        if (currentProfile.attackVfxPrefab != null)
        {
            Vector3 vfxPos = context.transform.position + context.transform.forward * 0.5f;
            Instantiate(currentProfile.attackVfxPrefab, vfxPos, Quaternion.identity);
        }

        // ƒл€ дальнего бо€ Ц если есть отдельный снар€д, спавним его
        if (!currentProfile.isMelee && currentProfile.projectilePrefab != null)
        {
            // »спользуем существующий MagAttack, но переопредел€ем его данные
            var magAttack = context.rangedAttack;
            if (magAttack != null)
            {
                // ѕример: метод ApplyProfile(profile)
                magAttack.ApplyProfile(currentProfile);
                magAttack.TryAttackPlayer(target);
            }
        }
        else
        {
            // Ѕлижн€€ атака Ц вызываем стандартную атаку врага
            context.enemyView?.Enemy?.Attack();
        }

        // Ёффект попадани€ будет обработан при ударе по игроку
    }
}