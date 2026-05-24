using UnityEngine;

public class AttackState : IState
{
    protected readonly EnemyContext context;
    protected readonly EnemyStateMachineAI ai;

    public AttackState(EnemyContext context, EnemyStateMachineAI ai)
    {
        this.context = context;
        this.ai = ai;
    }

    public virtual void Enter()
    {
        context.agent.isStopped = true;
        if (context.animator != null)
        {
            context.animator.Iswalking(false);
            context.animator.Isrunning(false);
        }
    }

    public virtual void Update()
    {
        if (context.IsDead)
        {
            ai.GetStateMachine().ChangeState(new DeathState(context, ai));
            return;
        }

        if (context.player == null)
        {
            ai.GetStateMachine().ChangeState(new IdleState(context, ai));
            return;
        }

        if (context.isPeaceful)
        {
            if (context.IsLowHealth)
                ai.GetStateMachine().ChangeState(new FleeState(context, ai));
            else
                ai.GetStateMachine().ChangeState(new IdleState(context, ai));
            return;
        }

        float dist = context.DistanceToPlayer;
        bool outOfMelee = context.HasMeleeAttack && dist > context.attackRange + context.attackRangeBuffer;
        bool outOfRanged = context.HasRangedAttack && (dist < context.rangedOptimalDistance - 2f || dist > context.rangedOptimalDistance + 2f);

        if (outOfMelee || outOfRanged)
        {
            ai.GetStateMachine().ChangeState(new ChaseState(context, ai));
            return;
        }

        RotateTowardsPlayer();

        if (context.attackHandler != null && context.attackHandler.CurrentProfile != null)
        {
            if (context.attackHandler.CanAttack)
            {
                context.attackHandler.PerformAttack(context, context.player);
                if (context.animator != null)
                    context.animator.PlayAttack();
            }
        }
        else
        {
            // Fallback на старую логику
            if (context.HasMeleeAttack)
                TryMeleeAttack_Fallback();
            else if (context.HasRangedAttack)
                TryRangedAttack_Fallback();
            else
                ai.GetStateMachine().ChangeState(new ChaseState(context, ai));
        }
    }

    private void TryMeleeAttack_Fallback()
    {
        if (context.meleeAttack == null) return;
        if (!context.meleeAttack.CanAttack) return;
        context.meleeAttack.TryAttackPlayer();
        if (context.animator != null) context.animator.PlayAttack();
    }

    private void TryRangedAttack_Fallback()
    {
        if (context.rangedAttack == null) return;
        if (!context.rangedAttack.CanAttack) return;
        context.rangedAttack.TryAttackPlayer(context.player);
        if (context.animator != null) context.animator.PlayAttack();
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