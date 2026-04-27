using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    protected EnemyContext context;
    private Vector3 roamTarget;

    public IdleState(EnemyContext context)
    {
        this.context = context;
    }

    public virtual void Enter()
    {
        roamTarget = GetRoamPosition();
        context.agent.speed = context.roamSpeed;
        context.agent.isStopped = false;
        context.animator.Iswalking(true);
        context.animator.Isrunning(false);
    }

    public virtual void Update()
    {
        // Движение к точке патрулирования
        if (Vector3.Distance(context.transform.position, roamTarget) < 1f)
            roamTarget = GetRoamPosition();

        context.agent.SetDestination(roamTarget);

        // Проверка условий выхода
        if (!context.isPeaceful && context.DistanceToPlayer <= context.chaseRange)
            context.GetComponent<StateMachine>().ChangeState(new ChaseState(context));
        else if (context.IsLowHealth)
            context.GetComponent<StateMachine>().ChangeState(new FleeState(context));
    }

    public virtual void Exit() { }

    private Vector3 GetRoamPosition()
    {
        Vector3 randomDir = Random.insideUnitSphere * Random.Range(5f, 15f);
        randomDir.y = 0;
        if (NavMesh.SamplePosition(context.transform.position + randomDir, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            return hit.position;
        return context.transform.position;
    }
}