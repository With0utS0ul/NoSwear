using UnityEngine;

public class DeathState : IState
{
    private EnemyContext context;

    public DeathState(EnemyContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        context.agent.isStopped = true;
        context.animator.SetDeath();
        GameObject.Destroy(context.gameObject, 2f);
    }

    public void Update() { }
    public void Exit() { }
}