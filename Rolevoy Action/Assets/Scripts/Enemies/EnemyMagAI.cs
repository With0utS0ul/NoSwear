using UnityEngine;
using UnityEngine.AI;

public class EnemyMagAI : MonoBehaviour, IDamageable
{
    [Header("Roaming")]
    [SerializeField] private float minWalkDistance = 5f;
    [SerializeField] private float maxWalkDistance = 15f;
    [SerializeField] private float reachedPointDistance = 1f;

    [Header("Detection")]
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float stopFollowRange = 25f;

    [Header("Ranged Combat")]
    [SerializeField] private float minAttackDistance = 5f;
    [SerializeField] private float maxAttackDistance = 10f;
    [SerializeField] private float optimalDistance = 7f;
    [SerializeField] private float repositionCooldown = 0.5f;

    [Header("Movement")]
    [SerializeField] private float roamSpeed = 2f;
    [SerializeField] private float combatSpeed = 3.5f;
    [SerializeField] private float retreatSpeed = 4f;

    [Header("Components")]
    [SerializeField] private MagAttack magAttack;
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private HealthComp healthComponent;

    private NavMeshAgent agent;
    private Transform player;
    private EnemyStates currentState;
    private Vector3 roamPosition;
    private float lastRepositionTime;
    private bool isAttacking;

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
            case EnemyStates.Following: UpdateRangedCombat(); break;
        }
    }

    private void UpdateRoaming()
    {
        if (Vector3.Distance(transform.position, roamPosition) <= reachedPointDistance)
            roamPosition = GenerateRoamPosition();

        agent.SetDestination(roamPosition);
        agent.speed = roamSpeed;
        agent.isStopped = false;

        animator.Iswalking(true);
        animator.Isrunning(false);

        if (Vector3.Distance(transform.position, player.position) <= followRange)
            currentState = EnemyStates.Following;
    }

    private void UpdateRangedCombat()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance >= stopFollowRange)
        {
            currentState = EnemyStates.Roaming;
            return;
        }

        if (Time.time - lastRepositionTime >= repositionCooldown)
        {
            if (distance < minAttackDistance)
            {
                isAttacking = false;
                Vector3 retreatDir = (transform.position - player.position).normalized;
                Vector3 targetPos = player.position + retreatDir * optimalDistance;
                SetDestination(targetPos);
                agent.speed = retreatSpeed;
                animator.Iswalking(false);
                animator.Isrunning(true);
            }
            else if (distance > maxAttackDistance)
            {
                isAttacking = false;
                Vector3 approachDir = (player.position - transform.position).normalized;
                Vector3 targetPos = player.position - approachDir * optimalDistance;
                SetDestination(targetPos);
                agent.speed = combatSpeed;
                animator.Iswalking(false);
                animator.Isrunning(true);
            }
            else
            {
                isAttacking = true;
                agent.isStopped = true;
                RotateTowardsPlayer();
                animator.Iswalking(false);
                animator.Isrunning(false);
                if (magAttack.CanAttack)
                {
                    magAttack.TryAttackPlayer(player);
                    animator.PlayAttack();
                }
                return;
            }
            lastRepositionTime = Time.time;
        }

        if (!isAttacking)
            agent.isStopped = false;
    }

    private void SetDestination(Vector3 target)
    {
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
        else
            agent.SetDestination(target);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
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