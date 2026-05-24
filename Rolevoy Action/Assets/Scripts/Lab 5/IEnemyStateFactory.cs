public interface IEnemyStateFactory
{
    IState GetInitialState();
    IState CreateIdleState();
    IState CreateChaseState();
    IState CreateAttackState();
    IState CreateFleeState();
    IState CreateDeathState();
    IState CreateStaggerState();
    IState CreateRageState();
    IState CreateBossHeavyAttackState();
}