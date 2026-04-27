using UnityEngine;

public class BossAttackState : IState
{
    protected EnemyContext context;
    protected EnemyStateMachineAI ai;
    protected float lastAttackTime;

    public BossAttackState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public virtual void Enter()
    {
        context.agent.isStopped = true;
        context.animator.Iswalking(false);
        context.animator.Isrunning(false);
    }

    public virtual void Update()
    {
        float dist = context.DistanceToPlayer;

        // Если игрок ушёл далеко – вернуться в Chase
        if (dist > context.attackRange * 1.5f)
        {
            ai.GetStateMachine().ChangeState(new BossChaseState(context, ai));
            return;
        }

        RotateTowardsPlayer();

        float cooldown = GetCooldown();
        if (Time.time >= lastAttackTime + cooldown)
        {
            // Выполнить атаку
            context.enemyView.Enemy.Attack();
            context.animator.PlayAttack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual float GetCooldown()
    {
        // Проверяем ХП < 50%: увеличиваем скорость атак (уменьшаем кулдаун)
        float healthPercent = context.enemyView.Enemy.Health.Current / context.enemyView.Enemy.Health.Max;
        if (healthPercent < 0.5f)
            return 0.75f;   // часто
        return 1.5f;        // обычно
    }

    protected void RotateTowardsPlayer()
    {
        Vector3 dir = (context.player.position - context.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    public virtual void Exit() { }
}