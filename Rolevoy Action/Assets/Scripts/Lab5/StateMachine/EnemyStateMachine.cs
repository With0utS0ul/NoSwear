// EnemyStateMachine.cs Ц машина состо€ний
public class EnemyStateMachine
{
    private EnemyState currentState;
    private EnemyState previousState;

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();
        previousState = currentState;
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void OnDamage()
    {
        currentState?.OnDamage();
    }

    public void RevertToPrevious()
    {
        if (previousState != null)
            ChangeState(previousState);
    }
}