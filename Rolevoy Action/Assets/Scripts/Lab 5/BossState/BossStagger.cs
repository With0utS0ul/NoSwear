using UnityEngine;
using System.Collections;

public class BossStaggerState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;
    private float staggerDuration = 1f;
    private float enterTime;

    public BossStaggerState(EnemyContext context, EnemyStateMachineAI ai)
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
            // Возврат в погоню или атаку в зависимости от дистанции
            if (context.DistanceToPlayer <= context.attackRange)
                ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
            else
                ai.GetStateMachine().ChangeState(new BossChaseState(context, ai));
        }
    }

    public void Exit()
    {
        context.agent.isStopped = false;
    }
}