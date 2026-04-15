// FleeState.cs – убегание от игрока
using UnityEngine;
using UnityEngine.AI;

public class FleeState : EnemyState
{
    private Vector3 fleeTarget;
    private float fleeSpeed = 5f;

    public override void Enter()
    {
        context.Agent.speed = fleeSpeed;
        context.Animator.SetBool("Isrunning", true);
        context.Animator.SetBool("Iswalking", false);
        UpdateFleeTarget();
    }

    private void UpdateFleeTarget()
    {
        Vector3 direction = (context.Agent.transform.position - context.Player.position).normalized;
        fleeTarget = context.Agent.transform.position + direction * 10f;
        // Проверка на NavMesh
        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            fleeTarget = hit.position;
        context.Agent.SetDestination(fleeTarget);
    }

    public override void Update()
    {
        float dist = Vector3.Distance(context.Agent.transform.position, context.Player.position);
        float healthPercent = context.Enemy.Health.Current / context.Enemy.Health.Max;

        if (healthPercent >= context.HealthThreshold || dist > context.FleeRange)
        {
            stateMachine.ChangeState(new IdleState());
            return;
        }
        if (Vector3.Distance(context.Agent.transform.position, fleeTarget) < 1f)
            UpdateFleeTarget();
    }
}