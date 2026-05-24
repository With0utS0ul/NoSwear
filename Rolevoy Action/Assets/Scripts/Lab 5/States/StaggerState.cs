using UnityEngine;

public class StaggerState : IState
{
    private readonly EnemyContext context;
    private readonly EnemyStateMachineAI ai;
    private float staggerDuration = 1f;
    private float enterTime;

    public StaggerState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
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
            if (context.DistanceToPlayer <= context.attackRange)
                ai.GetStateMachine().ChangeState(new AttackState(context, ai));
            else
                ai.GetStateMachine().ChangeState(new ChaseState(context, ai));
        }
    }

    public void Exit()
    {
        context.agent.isStopped = false;
    }
}