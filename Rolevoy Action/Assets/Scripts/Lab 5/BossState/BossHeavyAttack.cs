using UnityEngine;

public class BossHeavyAttack : BossAttackState
{
    private const float HeavyCooldown = 2.2f;

    public BossHeavyAttack(EnemyContext context, EnemyStateMachineAI ai) : base(context, ai) { }

    public override void Update()
    {
        if (context.IsDead)
        {
            ai.GetStateMachine().ChangeState(new BossDeathState(context, ai));
            return;
        }

        if (context.player == null)
        {
            ai.GetStateMachine().ChangeState(new BossIdleState(context, ai));
            return;
        }

        float dist = context.DistanceToPlayer;

        float effectiveRange = context.attackRange;
        if (context.bossCombatController?.CurrentProfile?.weaponType == BossWeaponType.Ranged)
            effectiveRange = context.rangedOptimalDistance;

        if (dist > effectiveRange + 1.0f)
        {
            ai.GetStateMachine().ChangeState(new BossChaseState(context, ai));
            return;
        }

        RotateTowardsPlayer();

        if (Time.time >= lastAttackTime + HeavyCooldown)
        {
            // поддержка ranged с проверкой готовности 
            if (context.bossCombatController?.CurrentProfile?.weaponType == BossWeaponType.Ranged)
            {
                if (context.bossCombatController.CanDoRangedAttack())
                {
                    context.bossCombatController.PerformRangedAttack(context.player);
                    if (context.animator != null)
                        context.animator.PlayBigAttack();
                    lastAttackTime = Time.time;
                }
                else
                {
                    // не можем выстрелить Ц остаЄмс€ в этом же состо€нии, не переключа€сь
                    return;
                }
            }
            else
            {
                context.bossCombatController?.PerformAttack(context.player);
                if (context.animator != null)
                    context.animator.PlayBigAttack();
                lastAttackTime = Time.time;
            }

            ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
        }
    }

    protected override float GetCooldown() => HeavyCooldown;
}