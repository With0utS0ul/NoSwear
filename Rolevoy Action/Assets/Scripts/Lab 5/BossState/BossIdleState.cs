using UnityEngine;

public class BossIdleState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;

    public BossIdleState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        context.agent.isStopped = true;
        context.animator.Iswalking(false);
        context.animator.Isrunning(false);
    }

    public void Update()
    {
        // Ќичего не делаем Ц босс стоит на месте
        // ¬ыход только через ai.ApplyDamage() который сменит состо€ние
    }

    public void Exit() { }
}