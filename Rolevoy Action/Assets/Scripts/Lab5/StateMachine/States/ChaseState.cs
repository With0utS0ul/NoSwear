using UnityEngine;
public class ChaseState : EnemyState
{
    public override void Enter()
    {
        context.Agent.isStopped = false;
        context.Agent.speed = 3.5f;
        context.Animator.SetBool("Isrunning", true);
        context.Animator.SetBool("Iswalking", false);
    }

    public override void Update()
    {
        float dist = Vector3.Distance(context.Agent.transform.position, context.Player.position);

        // Проверка на бегство при низком HP
        float healthPercent = context.Enemy.Health.Current / context.Enemy.Health.Max;
        if (healthPercent < context.HealthThreshold && dist < 10f)
        {
            stateMachine.ChangeState(new FleeState());
            return;
        }

        // Если игрок близко – атаковать
        if (dist < context.AttackRange)
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }
        // Если игрок далеко – преследовать
        if (dist > context.ChaseRange)
        {
            stateMachine.ChangeState(new IdleState());
            return;
        }

        context.Agent.SetDestination(context.Player.position);
    }

    public override void OnDamage()
    {
        // При получении урона в погоне – продолжаем погоню (можно добавить реакцию)
    }
}