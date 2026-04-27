using UnityEngine;

public class ChaseState : IState
{
    protected EnemyContext context;

    public ChaseState(EnemyContext context)
    {
        this.context = context;
    }

    public virtual void Enter()
    {
        context.agent.speed = context.chaseSpeed;
        context.agent.isStopped = false;
        context.animator.Iswalking(false);
        context.animator.Isrunning(true);
    }

    public virtual void Update()
    {
        float dist = context.DistanceToPlayer;

        if (context.IsLowHealth)
        {
            context.GetComponent<StateMachine>().ChangeState(new FleeState(context));
            return;
        }

        if (dist <= context.attackRange)
        {
            context.GetComponent<StateMachine>().ChangeState(new AttackState(context));
            return;
        }

        if (dist > context.stopChaseRange || (context.isPeaceful && dist > context.chaseRange))
        {
            context.GetComponent<StateMachine>().ChangeState(new IdleState(context));
            return;
        }

        // Преследование
        context.agent.SetDestination(context.player.position);
        RotateTowardsPlayer();
    }

    public virtual void Exit()
    {
        context.agent.isStopped = false;
    }

    protected void RotateTowardsPlayer()
    {
        Vector3 dir = (context.player.position - context.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
}