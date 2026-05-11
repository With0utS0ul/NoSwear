using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private readonly EnemyContext ctx;
    private Vector3 patrolTarget;
    private float nextRoamRefreshTime;

    public IdleState(EnemyContext context)
    {
        ctx = context;
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
            ctx.StateMachine.ChangeState(new DeathState(ctx));
            return;
        }

        if (ctx.player != null && !ctx.isPeaceful && ctx.DistanceToPlayer <= ctx.chaseRange)
        {
            ctx.StateMachine.ChangeState(new ChaseState(ctx));
            return;
        }

        if (ctx.isPeaceful && ctx.IsLowHealth)
        {
            ctx.StateMachine.ChangeState(new FleeState(ctx));
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