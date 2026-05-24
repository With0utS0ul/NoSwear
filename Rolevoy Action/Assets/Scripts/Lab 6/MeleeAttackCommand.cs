using UnityEngine;

public class MeleeAttackCommand : IAttackCommand
{
    public bool CanExecute(EnemyContext context)
    {
        return context.meleeAttack != null && context.meleeAttack.CanAttack;
    }

    public void Execute(EnemyContext context, Transform target)
    {
        context.meleeAttack.TryAttackPlayer();
    }
}