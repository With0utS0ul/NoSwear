// IdleState.cs
using UnityEngine;

public class IdleState : EnemyState
{
    private float idleTimer;
    private float idleDuration = 2f;

    public override void Enter()
    {
        idleTimer = 0f;
        context.Agent.isStopped = true;
        context.Animator.SetBool("Isrunning", false);
        context.Animator.SetBool("Iswalking", false);
    }

    public override void Update()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            // ¬место брожени€ Ц просто остаЄмс€ на месте, но можно добавить патруль
            idleTimer = 0f;
        }

        // ѕроверка: если игрок в радиусе агрессии и моб не в бегстве
        float dist = Vector3.Distance(context.Agent.transform.position, context.Player.position);
        if (dist < context.ChaseRange)
        {
            stateMachine.ChangeState(new ChaseState());
        }
    }

    public override void OnDamage()
    {
        // –еагирует на удар Ц переходит в агрессию (мирный режим: только если ударили)
        stateMachine.ChangeState(new ChaseState());
    }
}