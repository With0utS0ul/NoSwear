// EnemyState.cs Ц базовый класс состо€ни€
public abstract class EnemyState
{
    protected EnemyContext context;
    protected EnemyStateMachine stateMachine;

    public void Initialize(EnemyContext ctx, EnemyStateMachine sm)
    {
        context = ctx;
        stateMachine = sm;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void OnDamage() { } // вызываетс€ при получении урона
}
