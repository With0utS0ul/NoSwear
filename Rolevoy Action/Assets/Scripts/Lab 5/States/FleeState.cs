using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState
{
    private readonly EnemyContext ctx;
    private readonly EnemyStateMachineAI ai;
    private const float FleeDistance = 12f;
    private const float RepathInterval = 0.35f;
    private float startTime;
    private float nextRepathTime;

    public FleeState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.ctx = context;
        this.ai = ai;
    }

    public void Enter()
    {
        ctx.agent.isStopped = false;
        ctx.agent.speed = ctx.fleeSpeed;
        if (ctx.animator != null)
            ctx.animator.Isrunning(true);
        startTime = Time.time;
        nextRepathTime = Time.time;
        if (ctx.player == null)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
            return;
        }
        SetFleeDestination();
    }

    public void Update()
    {
        if (ctx.IsDead)
        {
            ai.GetStateMachine().ChangeState(new DeathState(ctx, ai));
            return;
        }

        if (!ctx.isPeaceful)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
            return;
        }

        if (ctx.player == null)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
            return;
        }

        if (Time.time >= nextRepathTime)
        {
            SetFleeDestination();
            nextRepathTime = Time.time + RepathInterval;
        }

        if (!ctx.IsLowHealth && Time.time > startTime + 1.0f)
        {
            ai.GetStateMachine().ChangeState(new IdleState(ctx, ai));
        }
    }

    public void Exit()
    {
        if (ctx.animator != null)
            ctx.animator.Isrunning(false);
    }

    private void SetFleeDestination()
    {
        Vector3 dir = (ctx.transform.position - ctx.player.position).normalized;
        Vector3 fleeTarget = ctx.transform.position + dir * FleeDistance;
        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            ctx.agent.SetDestination(hit.position);
        else
            ctx.agent.SetDestination(fleeTarget);
    }
}