using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState
{
    private EnemyContext context;
    private Vector3 fleeTarget;

    public FleeState(EnemyContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        context.agent.speed = context.fleeSpeed;
        context.animator.Iswalking(false);
        context.animator.Isrunning(true);
        SetFleeTarget();
    }

    public void Update()
    {
        if (!context.IsLowHealth)
        {
            context.GetComponent<StateMachine>().ChangeState(new IdleState(context));
            return;
        }

        if (Vector3.Distance(context.transform.position, fleeTarget) < 2f)
            SetFleeTarget();

        context.agent.SetDestination(fleeTarget);
    }

    private void SetFleeTarget()
    {
        Vector3 dirAway = (context.transform.position - context.player.position).normalized;
        Vector3 potentialTarget = context.transform.position + dirAway * 15f;
        if (NavMesh.SamplePosition(potentialTarget, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            fleeTarget = hit.position;
        else
            fleeTarget = context.transform.position;
    }

    public void Exit() { }
}