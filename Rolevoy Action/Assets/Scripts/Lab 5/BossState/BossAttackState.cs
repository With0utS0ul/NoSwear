using UnityEngine;

public class BossAttackState : IState
{
    protected EnemyContext context;
    protected EnemyStateMachineAI ai;
    protected float lastAttackTime;

    public BossAttackState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public virtual void Enter()
    {
        context.agent.isStopped = true;
        if (context.animator != null)
        {
            context.animator.Iswalking(false);
            context.animator.Isrunning(false);
        }
        lastAttackTime = Time.time - 999f;
    }

    public virtual void Update()
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
        float chaseThreshold = context.attackRange + context.attackRangeBuffer;

        if (context.bossCombatController?.CurrentProfile?.weaponType == BossWeaponType.Ranged)
        {
            effectiveRange = context.rangedOptimalDistance;
            chaseThreshold = effectiveRange + 2f;
        }

        if (dist > chaseThreshold)
        {
            ai.GetStateMachine().ChangeState(new BossChaseState(context, ai));
            return;
        }

        RotateTowardsPlayer();

        float cooldown = GetCooldown();
        if (Time.time >= lastAttackTime + cooldown)
        {
            if (ShouldUseHeavyAttack())
            {
                ai.GetStateMachine().ChangeState(new BossHeavyAttack(context, ai));
                return;
            }

            // роверка готовности дальнего боя
            if (context.bossCombatController?.CurrentProfile?.weaponType == BossWeaponType.Ranged)
            {
                if (context.bossCombatController.CanDoRangedAttack())
                {
                    context.bossCombatController.PerformRangedAttack(context.player);
                    if (context.animator != null)
                        context.animator.PlayAttack();
                    lastAttackTime = Time.time; // обновляем, только если была реальная атака
                }
                
            }
            else
            {
                // ближний бой
                context.bossCombatController?.PerformAttack(context.player);
                if (context.animator != null)
                    context.animator.PlayAttack();
                lastAttackTime = Time.time;
            }
           
        }
    }

    protected virtual float GetCooldown()
    {
        return context.BossAttackCooldown;
    }

    protected virtual bool ShouldUseHeavyAttack()
    {
        return Random.value < context.heavyAttackChance;
    }

    protected void RotateTowardsPlayer()
    {
        Vector3 dir = (context.player.position - context.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    public virtual void Exit() { }
}