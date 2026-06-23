using UnityEngine;

public class BossChaseState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;

    public BossChaseState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        context.agent.speed = context.chaseSpeed;
        context.agent.isStopped = false;
        if (context.animator != null)
        {
            context.animator.Iswalking(false);
            context.animator.Isrunning(true);
        }
    }

    public void Update()
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
        bool isRanged = context.bossCombatController?.CurrentProfile?.weaponType == BossWeaponType.Ranged;

        float attackRange = isRanged ? context.rangedOptimalDistance : context.attackRange;
        float stopDistance = isRanged ? attackRange + 2f : context.attackRange + context.attackRangeBuffer;

        if (dist <= stopDistance)
        {
            context.StateMachine.ChangeState(new BossAttackState(context, ai));
            return;
        }

        if (dist > context.stopChaseRange)
        {
            ai.GetStateMachine().ChangeState(new BossIdleState(context, ai));
            return;
        }

        context.agent.isStopped = false;
        context.agent.SetDestination(context.player.position);
        RotateTowardsPlayer();
    }

    public void Exit()
    {
        if (context.animator != null)
            context.animator.Isrunning(false);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dir = (context.player.position - context.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
}
