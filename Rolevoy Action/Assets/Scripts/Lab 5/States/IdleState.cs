using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private readonly EnemyContext ctx;
    private readonly EnemyStateMachineAI ai;
    private Vector3 patrolTarget;
    private float nextRoamRefreshTime;

    public IdleState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.ctx = context;
        this.ai = ai;
    }

    public void Enter()
    {
        patrolTarget = GetRandomPatrolPosition();
        ctx.agent.isStopped = false;
        ctx.agent.speed = ctx.roamSpeed;
        ctx.agent.SetDestination(patrolTarget);
        if (ctx.animator != null)
        {
            ctx.animator.Iswalking(true);
            ctx.animator.Isrunning(false);
        }
        nextRoamRefreshTime = Time.time + 1.5f;
    }

    public void Update()
    {
        if (ctx.IsDead)
        {
            ai.GetStateMachine().ChangeState(new DeathState(ctx, ai));
            return;
        }

        if (ctx.player != null && !ctx.isPeaceful && ctx.DistanceToPlayer <= ctx.chaseRange)
        {
            ai.GetStateMachine().ChangeState(new ChaseState(ctx, ai));
            return;
        }

        if (ctx.isPeaceful && ctx.IsLowHealth)
        {
            ai.GetStateMachine().ChangeState(new FleeState(ctx, ai));
            return;
        }

        bool reachedPoint = !ctx.agent.pathPending && ctx.agent.remainingDistance <= ctx.reachedRoamPointDistance;
        bool invalidPath = ctx.agent.pathStatus == NavMeshPathStatus.PathInvalid;
        bool stalePath = Time.time >= nextRoamRefreshTime && !ctx.agent.pathPending && ctx.agent.remainingDistance <= ctx.reachedRoamPointDistance + 0.25f;

        if (reachedPoint || invalidPath || stalePath)
        {
            patrolTarget = GetRandomPatrolPosition();
            ctx.agent.SetDestination(patrolTarget);
            nextRoamRefreshTime = Time.time + 1.5f;
        }
    }

    public void Exit()
    {
        if (ctx.animator != null)
            ctx.animator.Iswalking(false);
        if (ctx.agent != null)
            ctx.agent.isStopped = true;
    }

    private Vector3 GetRandomPatrolPosition()
    {
        Vector3 randomDir = Random.insideUnitSphere * Random.Range(ctx.minRoamDistance, ctx.maxRoamDistance);
        randomDir.y = 0;
        Vector3 newPos = ctx.transform.position + randomDir;
        if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            return hit.position;
        return ctx.transform.position;
    }
}