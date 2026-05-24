using UnityEngine;
using UnityEngine.AI;

public class BossIdleState : IState
{
    private readonly EnemyContext ctx;
    private readonly EnemyStateMachineAI ai;
    private Vector3 patrolTarget;
    private float nextPatrolRefreshTime;

    public BossIdleState(EnemyContext context, EnemyStateMachineAI ai)
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
        nextPatrolRefreshTime = Time.time + 1.5f;
    }

    public void Update()
    {
        if (ctx.IsDead)
        {
            ai.GetStateMachine().ChangeState(new BossDeathState(ctx, ai));
            return;
        }

        bool shouldAggroInNormalMode = !ctx.isPeaceful && ctx.player != null && ctx.DistanceToPlayer <= ctx.chaseRange;
        bool shouldAggroInPeacefulMode = ctx.isPeaceful && ai.IsAggro;

        if (shouldAggroInNormalMode || shouldAggroInPeacefulMode)
        {
            ai.GetStateMachine().ChangeState(new BossChaseState(ctx, ai));
            return;
        }

        bool reachedPoint = !ctx.agent.pathPending && ctx.agent.remainingDistance <= ctx.reachedRoamPointDistance;
        bool invalidPath = ctx.agent.pathStatus == NavMeshPathStatus.PathInvalid;
        bool stalePath = Time.time >= nextPatrolRefreshTime && !ctx.agent.pathPending && ctx.agent.remainingDistance <= ctx.reachedRoamPointDistance + 0.25f;

        if (reachedPoint || invalidPath || stalePath)
        {
            patrolTarget = GetRandomPatrolPosition();
            ctx.agent.SetDestination(patrolTarget);
            nextPatrolRefreshTime = Time.time + 1.5f;
        }
    }

    public void Exit()
    {
        if (ctx.animator != null)
            ctx.animator.Iswalking(false);
    }

    private Vector3 GetRandomPatrolPosition()
    {
        Vector3 randomDir = Random.insideUnitSphere * Random.Range(ctx.minRoamDistance, ctx.maxRoamDistance);
        randomDir.y = 0f;
        Vector3 target = ctx.transform.position + randomDir;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            return hit.position;
        return ctx.transform.position;
    }
}
