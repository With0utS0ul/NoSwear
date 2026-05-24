using UnityEngine;

public class RangedAttackCommand : IAttackCommand
{
    public bool CanExecute(EnemyContext context)
    {
        return context.rangedAttack != null && context.rangedAttack.CanAttack;
    }

    public void Execute(EnemyContext context, Transform target)
    {
        context.rangedAttack.TryAttackPlayer(target);
    }
}