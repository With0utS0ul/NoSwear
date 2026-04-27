using UnityEngine;

public class BossDeathState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;

    public BossDeathState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        context.agent.isStopped = true;
        context.animator.SetDeath();
        // Можно добавить задержку перед удалением
        Object.Destroy(context.gameObject, 2f);
    }

    public void Update() { }
    public void Exit() { }
}