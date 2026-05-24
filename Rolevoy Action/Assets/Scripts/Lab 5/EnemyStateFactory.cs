using UnityEngine;

public class EnemyStateFactory : IEnemyStateFactory
{
    private readonly EnemyContext context;
    private readonly EnemyStateMachineAI ai;
    private readonly bool isBoss;

    public EnemyStateFactory(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
        this.isBoss = context.GetComponent<BossTag>() != null;
    }

    public IState GetInitialState() => isBoss ? new BossIdleState(context, ai) : new IdleState(context, ai);
    public IState CreateIdleState() => isBoss ? new BossIdleState(context, ai) : new IdleState(context, ai);
    public IState CreateChaseState() => isBoss ? new BossChaseState(context, ai) : new ChaseState(context, ai);
    public IState CreateAttackState() => isBoss ? new BossAttackState(context, ai) : new AttackState(context, ai);
    public IState CreateFleeState() => new FleeState(context, ai);
    public IState CreateDeathState() => isBoss ? new BossDeathState(context, ai) : new DeathState(context, ai);
    public IState CreateStaggerState() => isBoss ? new BossStaggerState(context, ai) : new StaggerState(context, ai);
    public IState CreateRageState() => isBoss ? new BossRageState(context, ai) : new AttackState(context, ai);
    public IState CreateBossHeavyAttackState() => isBoss ? new BossHeavyAttack(context, ai) : new AttackState(context, ai);
}