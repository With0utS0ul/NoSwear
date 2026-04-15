// EnemyMeleeAI.cs (обновлённый)
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private EnemyAttack enemyAttack; // старый компонент

    [Header("Components")]
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private EnemyView enemyView;

    private NavMeshAgent agent;
    private Transform player;
    private EnemyStateMachine stateMachine;
    private EnemyContext context;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Создаём контекст
        context = new EnemyContext(
            agent, animator.GetComponent<Animator>(), player,
            enemyView.Enemy, enemyView, attackRange, chaseRange
        );
        stateMachine = new EnemyStateMachine();
        // Инициализируем состояния
        var idle = new IdleState();
        var chase = new ChaseState();
        var attack = new AttackState();
        var flee = new FleeState();
        idle.Initialize(context, stateMachine);
        chase.Initialize(context, stateMachine);
        attack.Initialize(context, stateMachine);
        flee.Initialize(context, stateMachine);
        stateMachine.ChangeState(idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    // Вызывается из EnemyDamageReceiver при получении урона
    public void OnDamaged()
    {
        stateMachine.OnDamage();
    }
}