using UnityEngine;

public class StateMachine
{
    private IState currentState;

    public IState GetCurrentState() => currentState;
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}