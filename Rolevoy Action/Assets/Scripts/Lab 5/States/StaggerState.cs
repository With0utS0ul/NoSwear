using System.Collections;
using UnityEngine;

public class StaggerState : IState
{
    private EnemyContext context;
    private float staggerDuration = 1f;
    private float enterTime;

    public StaggerState(EnemyContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        enterTime = Time.time;
        context.agent.isStopped = true;
        context.animator.SetGetDamage();
    }

    public void Update()
    {
        if (Time.time >= enterTime + staggerDuration)
        {
            // Возврат к прежнему состоянию (Chase или Attack)
            if (context.DistanceToPlayer <= context.attackRange)
                context.GetComponent<StateMachine>().ChangeState(new AttackState(context));
            else
                context.GetComponent<StateMachine>().ChangeState(new ChaseState(context));
        }
    }

    public void Exit()
    {
        context.agent.isStopped = false;
    }
}