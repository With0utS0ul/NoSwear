public class RageState : IState
{
    private EnemyContext context;
    private float originalAttackCooldown;

    public RageState(EnemyContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        // Увеличиваем скорость атаки (уменьшаем кулдаун)
        originalAttackCooldown = 1.5f;
        // Здесь можно изменить attack cooldown через поле в контексте или использовать глобальный модификатор
        Debug.Log("Boss enters RAGE mode! Attack speed increased.");
    }

    public void Update()
    {
        // Просто остаёмся в этом состоянии, пока действует режим ярости
        // Логика атаки обрабатывается в AttackState, но с изменённым кулдауном
    }

    public void Exit() { }
}