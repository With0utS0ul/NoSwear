using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    private readonly EnemyContext ctx;
    private readonly EnemyStateMachineAI ai;

    public ChaseState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.ctx = context;
        this.ai = ai;
    }

    public void Enter()
    {
        ctx.agent.isStopped = false;
        ctx.agent.speed = ctx.chaseSpeed;
        if (ctx.animator != null)
            ctx.animator.Isrunning(true);
    }

    public void Update()
    {
        if (ctx.IsDead)
        {
            ai.GetStateMachine().ChangeState(new DeathState(ctx, ai));
            return;
        }

        if (ctx.isPeaceful)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
            return;
        }

        if (ctx.player == null)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
            return;
        }

        float dist = ctx.DistanceToPlayer;

        if (dist > ctx.stopChaseRange)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
            return;
        }

        if (ctx.HasMeleeAttack && dist <= ctx.attackRange + ctx.attackRangeBuffer)
        {
            ai.GetStateMachine().ChangeState(new AttackState(ctx, ai));
            return;
        }

        if (ctx.HasRangedAttack)
        {
            float min = Mathf.Max(1f, ctx.rangedOptimalDistance - 1.5f);
            float max = ctx.rangedOptimalDistance + 1.5f;
            if (dist >= min && dist <= max)
            {
                ai.GetStateMachine().ChangeState(new AttackState(ctx, ai));
                return;
            }
            RepositionForRanged(dist);
        }
        else
        {
            ctx.agent.SetDestination(ctx.player.position);
        }
    }

    private void RepositionForRanged(float dist)
    {
        if (dist < ctx.rangedOptimalDistance - 1.5f)
        {
            Vector3 dir = (ctx.transform.position - ctx.player.position).normalized;
            Vector3 target = ctx.player.position + dir * ctx.rangedOptimalDistance;
            MoveTo(target);
            return;
        }

        Vector3 dirToPlayer = (ctx.player.position - ctx.transform.position).normalized;
        Vector3 approachTarget = ctx.player.position - dirToPlayer * ctx.rangedOptimalDistance;
        MoveTo(approachTarget);
    }

    private void MoveTo(Vector3 target)
    {
        ctx.agent.speed = ctx.chaseSpeed;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            ctx.agent.SetDestination(hit.position);
        else
            ctx.agent.SetDestination(target);
    }

    public void Exit()
    {
        if (ctx.animator != null)
            ctx.animator.Isrunning(false);
    }
}