// AttackState.cs
using UnityEngine;

public class AttackState : EnemyState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void Enter()
    {
        context.Agent.isStopped = true;
        context.Animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
        // Нанести удар
        context.View.Enemy.Attack();
    }

    public override void Update()
    {
        float dist = Vector3.Distance(context.Agent.transform.position, context.Player.position);
        if (dist > context.AttackRange + 1f)
        {
            stateMachine.ChangeState(new ChaseState());
            return;
        }
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Можно повторно атаковать
            lastAttackTime = Time.time;
            context.Animator.SetTrigger("Attack");
            context.View.Enemy.Attack();
        }
    }
}