using UnityEngine;

public class BossRageState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;
    private float rageDuration = 10f;
    private float rageStartTime;

    public BossRageState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        rageStartTime = Time.time;
        context.animator.Isrunning(true); 
    }

    public void Update()
    {
        if (Time.time >= rageStartTime + rageDuration)
        {
            // Âîçâðàò ê îáû÷íîìó ðåæèìó
            ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
            return;
        }

        
    }

    public void Exit() { }
}