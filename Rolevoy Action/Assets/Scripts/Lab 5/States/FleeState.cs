using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState
{
    private readonly EnemyContext ctx;
    private const float FleeDistance = 12f;
    private const float RepathInterval = 0.35f;
    private float startTime;
    private float nextRepathTime;

    public FleeState(EnemyContext context) => ctx = context;

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
            ctx.StateMachine.ChangeState(new IdleState(ctx));
            return;
        }
        SetFleeDestination();
    }

    public void Update()
    {
        if (ctx.IsDead)
        {
            ctx.StateMachine.ChangeState(new DeathState(ctx));
            return;
        }

        if (!ctx.isPeaceful)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
            return;
        }

        if (ctx.player == null)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
            return;
        }

        if (Time.time >= nextRepathTime)
        {
            SetFleeDestination();
            nextRepathTime = Time.time + RepathInterval;
        }

        if (!ctx.IsLowHealth && Time.time > startTime + 1.0f)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
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
