using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyContext), typeof(EnemyView))]
public class EnemyStateMachineAI : MonoBehaviour
{
    private StateMachine stateMachine;
    private EnemyContext context;
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
        context.StateMachine = stateMachine;
        isBoss = GetComponent<BossTag>() != null;

        context.agent = context.agent ??
                        GetComponent<NavMeshAgent>() ??
                        GetComponentInParent<NavMeshAgent>() ??
                        GetComponentInChildren<NavMeshAgent>();
        context.enemyView = context.enemyView ??
                           GetComponent<EnemyView>() ??
                           GetComponentInParent<EnemyView>() ??
                           GetComponentInChildren<EnemyView>();
        context.animator = context.animator ??
                           GetComponent<EnemyAnimator>() ??
                           GetComponentInParent<EnemyAnimator>() ??
                           GetComponentInChildren<EnemyAnimator>();
        if (context.meleeAttack == null)
            context.meleeAttack = GetComponent<EnemyAttack>() ?? GetComponentInParent<EnemyAttack>() ?? GetComponentInChildren<EnemyAttack>();
        if (context.rangedAttack == null)
            context.rangedAttack = GetComponent<MagAttack>() ?? GetComponentInParent<MagAttack>() ?? GetComponentInChildren<MagAttack>();
        context.player = ResolvePlayerTransform();

        if (context.agent == null)
        {
            Debug.LogError($"[{name}] EnemyStateMachineAI: NavMeshAgent not found. Attach AI to same prefab root as agent.");
            enabled = false;
            return;
        }

        // Для обычных врагов
        if (!isBoss)
        {
            var handler = GetComponent<EnemyAttackHandler>();
            if (handler == null) handler = gameObject.AddComponent<EnemyAttackHandler>();
            context.attackHandler = handler;
            // Загружаем все профили из папки Resources/AttackProfiles
            AttackProfile[] allProfiles = Resources.LoadAll<AttackProfile>("AttackProfiles");
            // Фильтруем по ближнему/дальнему (определяем по наличию MagAttack или по тегу)
            bool isMeleeEnemy = GetComponent<MagAttack>() == null; // если нет MagAttack – ближний
            var suitable = System.Array.FindAll(allProfiles, p => p.isMelee == isMeleeEnemy);

            if (suitable.Length > 0)
            {
                var randomProfile = suitable[Random.Range(0, suitable.Length)];
                handler.SetProfile(randomProfile);
            }
        }

        bool servicePeaceful = GameEntryPoint.Instance?.PeacefulModeService?.IsPeaceful ?? false;
        context.isPeaceful = servicePeaceful;

        if (isBoss)
            stateMachine.ChangeState(new BossIdleState(context, this));
        else
            stateMachine.ChangeState(new IdleState(context));
    }

    private void Update()
    {
        if (context == null || context.agent == null)
            return;

        if (!context.agent.enabled)
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
                stateMachine.ChangeState(new BossChaseState(context, this));
            return;
        }

        if (isBoss)
        {
            return;
        }

        if (context.isPeaceful && context.IsLowHealth)
            stateMachine.ChangeState(new FleeState(context));
    }

    public bool IsBoss => isBoss;
    public bool IsAggro => isAggro;
    public StateMachine GetStateMachine() => stateMachine;

    private Transform ResolvePlayerTransform()
    {
        var byTag = GameObject.FindGameObjectWithTag("Player");
        if (byTag != null)
            return byTag.transform;

        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
            return playerController.transform;

        return null;
    }
}
