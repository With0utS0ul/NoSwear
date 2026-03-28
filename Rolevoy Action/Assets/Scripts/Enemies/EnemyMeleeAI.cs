using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAI : MonoBehaviour, IDamageable
{
    [Header("Roaming")]
    [SerializeField] private float minWalkDistance = 5f;
    [SerializeField] private float maxWalkDistance = 15f;
    [SerializeField] private float reachedPointDistance = 1f;

    [Header("Detection")]
    [SerializeField] private float followRange = 10f;
    [SerializeField] private float stopFollowRange = 20f;

    [Header("Combat")]
    [SerializeField] private EnemyAttack enemyAttack;

    [Header("Components")]
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private HealthComp healthComponent;

    private NavMeshAgent agent;
    private Transform player;
    private EnemyStates currentState;
    private Vector3 roamPosition;

    private enum EnemyStates { Roaming, Following }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyStates.Roaming;
        roamPosition = GenerateRoamPosition();

        healthComponent.OnDeath += Die;
        healthComponent.OnHealthChanged += _ => animator.SetGetDamage();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Roaming: UpdateRoaming(); break;
            case EnemyStates.Following: UpdateFollowing(); break;
        }
    }

    private void UpdateRoaming()
    {
        if (Vector3.Distance(transform.position, roamPosition) <= reachedPointDistance)
            roamPosition = GenerateRoamPosition();

        agent.SetDestination(roamPosition);
        agent.speed = 2f;
        agent.isStopped = false;

        animator.Iswalking(true);
        animator.Isrunning(false);

        if (Vector3.Distance(transform.position, player.position) <= followRange)
            currentState = EnemyStates.Following;
    }

    private void UpdateFollowing()
    {
        agent.SetDestination(player.position);
        agent.speed = 3f;
        agent.isStopped = false;

        animator.Iswalking(false);
        animator.Isrunning(true);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= enemyAttack.AttackRange && enemyAttack.CanAttack)
        {
            agent.isStopped = true;
            enemyAttack.TryAttackPlayer();
            animator.PlayAttack();
        }
        else if (distance >= stopFollowRange)
            currentState = EnemyStates.Roaming;
    }

    private Vector3 GenerateRoamPosition()
    {
        Vector3 randomDir = Random.insideUnitSphere * Random.Range(minWalkDistance, maxWalkDistance);
        randomDir.y = 0;
        Vector3 newPos = transform.position + randomDir;
        if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            return hit.position;
        return transform.position;
    }

    private void Die()
    {
        animator.SetDeath();
        Destroy(gameObject, 2f);
    }

    public void ApplyDamage(Damage damage)
    {
        float total = damage.Physical + damage.Magical;
        healthComponent.Take(total);
    }
}