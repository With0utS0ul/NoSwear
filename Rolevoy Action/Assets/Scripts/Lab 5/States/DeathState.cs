using UnityEngine;

public class DeathState : IState
{
    private readonly EnemyContext context;
    private bool entered;

    public DeathState(EnemyContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        if (entered) return;
        entered = true;
        if (context.agent != null)
            context.agent.isStopped = true;
        if (context.animator != null)
            context.animator.SetDeath();
    }

    public void Update() { }
    public void Exit() { }
}