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

        if (dist > context.attackRange + 1.0f)
        {
            ai.GetStateMachine().ChangeState(new BossChaseState(context, ai));
            return;
        }

        RotateTowardsPlayer();

        if (Time.time >= lastAttackTime + HeavyCooldown)
        {
            context.enemyView.Enemy.Attack();
            if (context.animator != null)
                context.animator.PlayBigAttack();
            lastAttackTime = Time.time;

            ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
        }
    }

    protected override float GetCooldown() => HeavyCooldown;
}
