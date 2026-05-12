using UnityEngine;

public class AttackState : IState
{
    private readonly EnemyContext context;
    private float lastAttackTime; // уже не нужен, но оставим для совместимости (можно удалить)

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

        // ★★★ Главные изменения ★★★
        // Пытаемся атаковать через AttackHandler
        if (context.attackHandler != null && context.attackHandler.CurrentProfile != null)
        {
            if (context.attackHandler.CanAttack) // проверяем кулдаун внутри хендлера
            {
                context.attackHandler.PerformAttack(context, context.player);
                // Анимацию запускаем здесь, если её нет внутри PerformAttack
                if (context.animator != null)
                    context.animator.PlayAttack();
            }
        }
        else
        {
            // Fallback на старую логику (если хендлер или профиль отсутствуют)
            if (context.HasMeleeAttack)
                TryMeleeAttack_Fallback();
            else if (context.HasRangedAttack)
                TryRangedAttack_Fallback();
            else
                context.StateMachine.ChangeState(new ChaseState(context));
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