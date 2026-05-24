public class RageState : IState
{
    private readonly EnemyContext context;
    private readonly EnemyStateMachineAI ai;
    private float originalAttackCooldown;

    public RageState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        originalAttackCooldown = 0.5f;
        // логика изменения кулдауна
    }

    public void Update() { }

    public void Exit() { }
}