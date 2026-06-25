using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField] private AttackProfile currentProfile;
    private float lastAttackTime;
    private IAttackCommand currentAttackCommand;

    public AttackProfile CurrentProfile => currentProfile;

    public void SetProfile(AttackProfile profile)
    {
        currentProfile = profile;
        currentAttackCommand = profile.isMelee ? new MeleeAttackCommand() : new RangedAttackCommand();
    }

    public bool CanAttack => Time.time >= lastAttackTime + currentProfile.cooldown;

    public void PerformAttack(EnemyContext context, Transform target)
    {
        if (!CanAttack) return;
        if (currentAttackCommand == null || !currentAttackCommand.CanExecute(context)) return;

        currentAttackCommand.Execute(context, target);
        lastAttackTime = Time.time;

        if (currentProfile.attackSound != null)
            AudioSource.PlayClipAtPoint(currentProfile.attackSound, context.transform.position);

        if (currentProfile.attackVfxPrefab != null)
        {
            Vector3 vfxPos = context.transform.position + context.transform.forward * 0.5f;
            Instantiate(currentProfile.attackVfxPrefab, vfxPos, Quaternion.identity);
        }

        if (!currentProfile.isMelee && context.rangedAttack != null)
            context.rangedAttack.ApplyProfile(currentProfile);
        if (context.animator != null)
            context.animator.PlayAttack();
    }
}