using UnityEngine;

public class BossHeavyAttack : BossAttackState
{
    private float heavyCooldown = 3f;
    private float lastHeavyAttack;
    private float heavyDamageMultiplier = 2f; // например, двойной урон

    public BossHeavyAttack(EnemyContext context, EnemyStateMachineAI ai) : base(context, ai) { }

    public override void Enter()
    {
        base.Enter();
        // Можно проиграть анимацию HeavyAttack
        context.animator.PlayAttack(); // замените на специальный триггер, если есть
    }

    public override void Update()
    {
        float dist = context.DistanceToPlayer;

        if (dist > context.attackRange * 1.5f)
        {
            ai.GetStateMachine().ChangeState(new BossChaseState(context, ai));
            return;
        }

        RotateTowardsPlayer();

        if (Time.time >= lastHeavyAttack + heavyCooldown)
        {
            // Применить усиленный урон
            // Для простоты будем вызывать обычную атаку, но можно создать отдельный метод
            context.enemyView.Enemy.Attack(); // здесь нужно, чтобы урон был увеличен
            // Лучше через отдельный метод в Enemy или использовать параметр
            Debug.Log("Heavy attack! Double damage.");
            lastHeavyAttack = Time.time;

            // После сильной атаки – вернуться к обычным атакам
            ai.GetStateMachine().ChangeState(new BossAttackState(context, ai));
        }
    }

    protected override float GetCooldown()
    {
        // Сильная атака не ускоряется от здоровья
        return heavyCooldown;
    }
}