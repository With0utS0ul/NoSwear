using UnityEngine;
using UnityEngine.AI;

public class EnemyContext : MonoBehaviour, IEnemyPeacefulHandler
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Transform player;
    public EnemyView enemyView;
    public EnemyAnimator animator;
    public EnemyAttack meleeAttack;      // дл€ ближнего бо€
    public MagAttack rangedAttack;       // дл€ дальнего бо€

    [Header("Settings")]
    public bool isPeaceful = true;       // мирный режим Ц не агритс€
    public float healthThresholdToFlee = 0.3f; // % HP дл€ бегства

    [Header("Detection")]
    public float chaseRange = 15f;
    public float stopChaseRange = 25f;
    public float attackRange = 2f;       // дл€ ближней атаки
    public float rangedOptimalDistance = 7f;

    [Header("Movement")]
    public float roamSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float fleeSpeed = 5f;

    public bool IsLowHealth => enemyView.Enemy.Health.Current / enemyView.Enemy.Health.Max < healthThresholdToFlee;
    public float DistanceToPlayer => Vector3.Distance(transform.position, player.position);
    private void Start()
    {
        // –егистрируемс€ в сервисе мирного режима
        GameEntryPoint.Instance?.PeacefulModeService?.RegisterEnemy(this);
    }

    private void OnDestroy()
    {
        GameEntryPoint.Instance?.PeacefulModeService?.UnregisterEnemy(this);
    }

    public void OnPeacefulModeChanged(bool isPeaceful)
    {
        this.isPeaceful = isPeaceful;

        // ≈сли мирный режим включЄн, а враг находилс€ в Chase/Attack Ц переводим в Idle
        if (isPeaceful)
        {
            var stateMachine = GetComponent<EnemyStateMachineAI>()?.GetStateMachine();
            if (stateMachine != null && !(stateMachine.GetCurrentState() is IdleState))
            {
                // »збегаем конфликтов Ц только дл€ обычных мобов (не боссов)
                if (!GetComponent<BossTag>())
                    stateMachine.ChangeState(new IdleState(this));
            }
        }
        // ≈сли мирный режим выключен Ц ничего не делаем, враг сам заагритс€ при приближении
    }
}