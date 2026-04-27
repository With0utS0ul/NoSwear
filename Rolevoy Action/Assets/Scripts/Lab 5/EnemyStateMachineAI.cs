using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyContext), typeof(EnemyView))]
public class EnemyStateMachineAI : MonoBehaviour
{
    private StateMachine stateMachine;
    private EnemyContext context;
    private bool isBoss;
    private bool isAggro = false;          // дл€ босса Ц агр включаетс€ после удара

    private void Start()
    {
        stateMachine = new StateMachine();
        context = GetComponent<EnemyContext>();
        isBoss = GetComponent<BossTag>() != null;

        context.player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (context.player == null) return;

        context.agent = GetComponent<NavMeshAgent>();
        context.enemyView = GetComponent<EnemyView>();
        context.animator = GetComponent<EnemyAnimator>();

        // ƒл€ обычных мобов включаем мирный режим по умолчанию
        if (!isBoss) context.isPeaceful = true;

        if (isBoss)
            stateMachine.ChangeState(new BossIdleState(context, this));
        else
            stateMachine.ChangeState(new IdleState(context));
    }

    private void Update()
    {
        if (context.player == null) return;
        stateMachine.Update();
    }

    public void ApplyDamage(Damage damage)
    {
        if (isBoss && !isAggro)
        {
            isAggro = true;
            context.isPeaceful = false;
            if (stateMachine.GetCurrentState() is BossIdleState)
                stateMachine.ChangeState(new BossChaseState(context, this));
        }
        // ƒл€ обычных мобов Ц ничего не делаем (они остаютс€ мирными)
    }
    // ¬ызываетс€ из EnemyDamageReceiver при получении урона
    public void OnDamageReceived(Damage damage)
    {
        if (isBoss && !isAggro)
        {
            isAggro = true;
            context.isPeaceful = false;   // босс больше не мирный
            // переходим в агрессию
            if (stateMachine.GetCurrentState() is BossIdleState)
                stateMachine.ChangeState(new BossChaseState(context, this));
        }
        // ƒл€ обычных мобов Ц ничего не делаем, они остаютс€ мирными (даже при атаке)
    }

    public bool IsBoss => isBoss;
    public bool IsAggro => isAggro;
    public StateMachine GetStateMachine() => stateMachine;
}