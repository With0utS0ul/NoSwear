using UnityEngine;

public class HeavyAttackState : AttackState
{
    private float heavyCooldown = 3f;
    private float lastHeavyAttack;

    public HeavyAttackState(EnemyContext context) : base(context) { }

    public override void Enter()
    {
        base.Enter();
        // ћожно проиграть другую анимацию или увеличить урон
        context.animator.PlayAttack(); // замените на HeavyAttack при необходимости
    }

    public override void Update()
    {
        float dist = context.DistanceToPlayer;
        if (dist > context.attackRange * 1.5f)
        {
            context.GetComponent<StateMachine>().ChangeState(new ChaseState(context));
            return;
        }

        RotateTowardsPlayer();
        if (Time.time >= lastHeavyAttack + heavyCooldown)
        {
            // «десь можно нанести больший урон
            context.enemyView.Enemy.Attack();
            lastHeavyAttack = Time.time;
        }
    }

    protected override float GetCooldown() => heavyCooldown;
}