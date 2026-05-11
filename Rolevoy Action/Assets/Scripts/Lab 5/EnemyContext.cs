using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContext : MonoBehaviour, IEnemyPeacefulHandler
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Transform player;
    public EnemyView enemyView;
    public EnemyAnimator animator;
    public EnemyAttack meleeAttack;
    public MagAttack rangedAttack;

    [Header("Settings")]
    public bool isPeaceful = true;
    public float healthThresholdToFlee = 0.5f;
    public float attackRangeBuffer = 0.5f;
    public float heavyAttackChance = 0.25f;
    public float bossAttackCooldownNormal = 1.5f;
    public float bossAttackCooldownEnraged = 0.8f;

    [Header("Detection")]
    public float chaseRange = 15f;
    public float stopChaseRange = 25f;
    public float attackRange = 2f;
    public float rangedOptimalDistance = 7f;

    [Header("Movement")]
    public float roamSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float fleeSpeed = 5f;
    public float minRoamDistance = 5f;
    public float maxRoamDistance = 15f;
    public float reachedRoamPointDistance = 1f;

    public StateMachine StateMachine { get; set; }

    public bool IsLowHealth =>
        enemyView != null &&
        enemyView.Enemy != null &&
        enemyView.Enemy.Health != null &&
        enemyView.Enemy.Health.Max > 0f &&
        enemyView.Enemy.Health.Current / enemyView.Enemy.Health.Max < healthThresholdToFlee;

    public bool IsDead =>
        enemyView != null &&
        enemyView.Enemy != null &&
        enemyView.Enemy.Health != null &&
        enemyView.Enemy.Health.Current <= 0f;

    public bool HasMeleeAttack => meleeAttack != null;
    public bool HasRangedAttack => rangedAttack != null;
    public float DistanceToPlayer => player == null ? float.MaxValue : Vector3.Distance(transform.position, player.position);
    public bool HasValidAttack => attackHandler != null && attackHandler.CurrentProfile != null;
    public bool CanAttackNow =>
        HasMeleeAttack ? meleeAttack.CanAttack :
        HasRangedAttack && rangedAttack.CanAttack;

    public float BossAttackCooldown
    {
        get
        {
            if (enemyView == null || enemyView.Enemy == null || enemyView.Enemy.Health == null || enemyView.Enemy.Health.Max <= 0f)
                return bossAttackCooldownNormal;
            float hpPercent = enemyView.Enemy.Health.Current / enemyView.Enemy.Health.Max;
            return hpPercent < 0.5f ? bossAttackCooldownEnraged : bossAttackCooldownNormal;
        }
    }

    public EnemyAttackHandler attackHandler;
    public BossCombatController bossCombatController;
    private void Start()
    {
        GameEntryPoint.Instance?.PeacefulModeService?.RegisterEnemy(this);
    }

    private void OnDestroy()
    {
        GameEntryPoint.Instance?.PeacefulModeService?.UnregisterEnemy(this);
    }

    public void OnPeacefulModeChanged(bool isPeaceful)
    {
        this.isPeaceful = isPeaceful;
    }
}
