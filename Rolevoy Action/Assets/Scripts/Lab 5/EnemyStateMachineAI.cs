using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyContext), typeof(EnemyView))]
public class EnemyStateMachineAI : MonoBehaviour
{
    private StateMachine stateMachine;
    private EnemyContext context;
    private IEnemyStateFactory stateFactory;
    private bool isBoss;
    private bool isAggro = false;

    private void Start()
    {
        context = GetComponent<EnemyContext>() ?? GetComponentInParent<EnemyContext>() ?? GetComponentInChildren<EnemyContext>();
        if (context == null)
        {
            enabled = false;
            return;
        }

        stateMachine = new StateMachine();
        
        isBoss = GetComponent<BossTag>() != null;

        // Инициализация компонентов
        context.agent = context.agent ?? GetComponent<NavMeshAgent>() ?? GetComponentInParent<NavMeshAgent>() ?? GetComponentInChildren<NavMeshAgent>();
        context.enemyView = context.enemyView ?? GetComponent<EnemyView>() ?? GetComponentInParent<EnemyView>() ?? GetComponentInChildren<EnemyView>();
        context.animator = context.animator ?? GetComponent<EnemyAnimator>() ?? GetComponentInParent<EnemyAnimator>() ?? GetComponentInChildren<EnemyAnimator>();
        if (context.meleeAttack == null)
            context.meleeAttack = GetComponent<EnemyAttack>() ?? GetComponentInParent<EnemyAttack>() ?? GetComponentInChildren<EnemyAttack>();
        if (context.rangedAttack == null)
            context.rangedAttack = GetComponent<MagAttack>() ?? GetComponentInParent<MagAttack>() ?? GetComponentInChildren<MagAttack>();
        context.player = ResolvePlayerTransform();

        if (context.agent == null)
        {
            Debug.LogError($"[{name}] EnemyStateMachineAI: NavMeshAgent not found.");
            enabled = false;
            return;
        }

        // Инициализация AttackHandler для обычных врагов
        if (!isBoss)
        {
            var handler = GetComponent<EnemyAttackHandler>();
            if (handler == null) handler = gameObject.AddComponent<EnemyAttackHandler>();
            context.attackHandler = handler;
            AttackProfile[] allProfiles = Resources.LoadAll<AttackProfile>("AttackProfiles");
            bool isMeleeEnemy = GetComponent<MagAttack>() == null;
            var suitable = System.Array.FindAll(allProfiles, p => p.isMelee == isMeleeEnemy);
            if (suitable.Length > 0)
            {
                var randomProfile = suitable[Random.Range(0, suitable.Length)];
                handler.SetProfile(randomProfile);
            }
        }

        bool servicePeaceful = GameEntryPoint.Instance?.PeacefulModeService?.IsPeaceful ?? false;
        context.isPeaceful = servicePeaceful;

        // Используем фабрику для создания начального состояния
        stateFactory = new EnemyStateFactory(context, this);
        IState initialState = stateFactory.GetInitialState();
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        if (context == null || context.agent == null || !context.agent.enabled)
            return;

        if (!context.agent.isOnNavMesh && NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            context.agent.Warp(hit.position);

        if (!context.agent.isOnNavMesh)
            return;

        if (context.player == null)
            context.player = ResolvePlayerTransform();

        stateMachine.Update();
    }

    public void OnDamageReceived(Damage damage)
    {
        if (context == null || context.IsDead)
            return;

        if (isBoss && !isAggro)
        {
            isAggro = true;
            if (!(stateMachine.GetCurrentState() is BossDeathState))
                stateMachine.ChangeState(stateFactory.CreateChaseState());
            return;
        }

        if (isBoss)
            return;

        if (context.isPeaceful && context.IsLowHealth)
            stateMachine.ChangeState(stateFactory.CreateFleeState());
    }

    public bool IsBoss => isBoss;
    public bool IsAggro => isAggro;
    public StateMachine GetStateMachine() => stateMachine;
    public IEnemyStateFactory StateFactory => stateFactory; 

    private Transform ResolvePlayerTransform()
    {
        var byTag = GameObject.FindGameObjectWithTag("Player");
        if (byTag != null) return byTag.transform;
        var playerController = FindObjectOfType<PlayerController>();
        return playerController != null ? playerController.transform : null;
    }
}