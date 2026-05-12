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

        // Наносим урон (через старые компоненты, чтобы не ломать существующую логику)
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

        if (currentProfile.attackSound != null)
            AudioSource.PlayClipAtPoint(currentProfile.attackSound, context.transform.position);

        if (currentProfile.attackVfxPrefab != null)
        {
            Vector3 vfxPos = context.transform.position + context.transform.forward * 0.5f;
            Instantiate(currentProfile.attackVfxPrefab, vfxPos, Quaternion.identity);
        }

        if (!currentProfile.isMelee && currentProfile.projectilePrefab != null)
        {
            // Можно создать временный снаряд или переопределить параметры MagAttack
            // Например, через отдельный метод в MagAttack
            var magAttack = context.rangedAttack;
            if (magAttack != null)
            {
                magAttack.ApplyProfile(currentProfile); // этот метод нужно реализовать в MagAttack
                magAttack.TryAttackPlayer(target);
            }
        }

        // Анимация – вызываем здесь (или оставляем в AttackState – на ваше усмотрение)
        if (context.animator != null)
            context.animator.PlayAttack();
    }
}