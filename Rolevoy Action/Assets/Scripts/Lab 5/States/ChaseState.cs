using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    private readonly EnemyContext ctx;

    public ChaseState(EnemyContext context) => ctx = context;

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
            ctx.StateMachine.ChangeState(new DeathState(ctx));
            return;
        }

        if (ctx.isPeaceful)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
            return;
        }

        if (ctx.player == null)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
            return;
        }

        float dist = ctx.DistanceToPlayer;

        if (dist > ctx.stopChaseRange)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
            return;
        }

        if (ctx.HasMeleeAttack && dist <= ctx.attackRange + ctx.attackRangeBuffer)
        {
            ctx.StateMachine.ChangeState(new AttackState(ctx));
            return;
        }

        if (ctx.HasRangedAttack)
        {
            float min = Mathf.Max(1f, ctx.rangedOptimalDistance - 1.5f);
            float max = ctx.rangedOptimalDistance + 1.5f;
            if (dist >= min && dist <= max)
            {
                ctx.StateMachine.ChangeState(new AttackState(ctx));
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