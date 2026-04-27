using UnityEngine;

public class AttackState : IState
{
    protected EnemyContext context;
    protected float lastAttackTime;

    public AttackState(EnemyContext context)
    {
        this.context = context;
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

        if (context.IsLowHealth)
        {
            context.GetComponent<StateMachine>().ChangeState(new FleeState(context));
            return;
        }

        if (dist > context.attackRange * 1.2f)
        {
            context.GetComponent<StateMachine>().ChangeState(new ChaseState(context));
            return;
        }

        RotateTowardsPlayer();

        float cooldown = GetCooldown();
        if (Time.time >= lastAttackTime + cooldown)
        {
            context.enemyView.Enemy.Attack();
            context.animator.PlayAttack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual float GetCooldown()
    {
        // Ѕазова€ задержка Ц можно переопределить в боссе
        return 1.5f;
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