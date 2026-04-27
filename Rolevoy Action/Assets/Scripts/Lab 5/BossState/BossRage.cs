using UnityEngine;

public class BossRageState : IState
{
    private EnemyContext context;
    private EnemyStateMachineAI ai;
    private float rageDuration = 10f;
    private float rageStartTime;

    public BossRageState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public void Enter()
    {
        rageStartTime = Time.time;
        context.animator.Isrunning(true); // визуальный эффект
        Debug.Log("Boss enters RAGE mode!");
        // Увеличиваем скорость атаки (переопределим в атаках через проверку состояния)
    }

    public void Update()
    {
        if (Time.time >= rageStartTime + rageDuration)
        {
            // Возврат к обычному режиму
            ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
            return;
        }

        // В состоянии ярости – продолжаем атаковать (можно перейти в BossAttackState, но с флагом)
        // Для простоты: оставляем текущее состояние, но атаки обрабатываются через отдельный механизм.
        // Лучше перейти в BossAttackState, но с модификатором.
    }

    public void Exit() { }
}