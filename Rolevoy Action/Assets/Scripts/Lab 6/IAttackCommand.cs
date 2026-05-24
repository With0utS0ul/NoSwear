using UnityEngine;

public interface IAttackCommand
{
    bool CanExecute(EnemyContext context);
    void Execute(EnemyContext context, Transform target);
}