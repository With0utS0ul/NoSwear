using UnityEngine;

public class AttackState : IState
{
    private readonly EnemyContext context;
    private float lastAttackTime;

    public AttackState(EnemyContext context)
    {
        this.context = context;
    }

    public virtual void Enter()
    {
        context.agent.isStopped = true;
        if (context.animator != null)
        {
            context.animator.Iswalking(false);
            context.animator.Isrunning(false);
        }
        lastAttackTime = Time.time - 999f;
    }

    public virtual void Update()
    {
        if (context.IsDead)
        {
            context.StateMachine.ChangeState(new DeathState(context));
            return;
        }

        if (context.player == null)
        {
            context.StateMachine.ChangeState(new IdleState(context));
            return;
        }

        if (context.isPeaceful)
        {
            if (context.IsLowHealth)
                context.StateMachine.ChangeState(new FleeState(context));
            else
                context.StateMachine.ChangeState(new IdleState(context));
            return;
        }

        float dist = context.DistanceToPlayer;
        bool outOfMelee = context.HasMeleeAttack && dist > context.attackRange + context.attackRangeBuffer;
        bool outOfRanged = context.HasRangedAttack && (dist < context.rangedOptimalDistance - 2f || dist > context.rangedOptimalDistance + 2f);

        if (outOfMelee || outOfRanged)
        {
            context.StateMachine.ChangeState(new ChaseState(context));
            return;
        }

        RotateTowardsPlayer();

        if (context.HasMeleeAttack)
        {
            TryMeleeAttack();
            return;
        }

        if (context.HasRangedAttack)
        {
            TryRangedAttack();
            return;
        }

        context.StateMachine.ChangeState(new ChaseState(context));
    }

    private void TryMeleeAttack()
    {
        if (context.meleeAttack == null)
            return;
        if (Time.time < lastAttackTime + context.meleeAttack.CoolDown)
            return;
        if (!context.meleeAttack.CanAttack)
            return;

        context.meleeAttack.TryAttackPlayer();
        if (context.animator != null)
            context.animator.PlayAttack();
        lastAttackTime = Time.time;
    }

    private void TryRangedAttack()
    {
        if (context.rangedAttack == null)
            return;
        if (Time.time < lastAttackTime + context.rangedAttack.CoolDown)
            return;
        if (!context.rangedAttack.CanAttack)
            return;

        context.rangedAttack.TryAttackPlayer(context.player);
        if (context.animator != null)
            context.animator.PlayAttack();
        lastAttackTime = Time.time;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dir = (context.player.position - context.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    public virtual void Exit()
    {
        context.agent.isStopped = false;
    }
}