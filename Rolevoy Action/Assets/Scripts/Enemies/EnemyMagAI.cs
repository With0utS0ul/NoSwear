using UnityEngine;
using UnityEngine.AI;

public class EnemyMagAI : MonoBehaviour
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

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Movement")]
    [SerializeField] private float roamSpeed = 2f;
    [SerializeField] private float combatSpeed = 3.5f;
    [SerializeField] private float retreatSpeed = 4f;

    [Header("Components")]
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private EnemyView enemyView;

    private NavMeshAgent agent;
    private Transform player;
    private EnemyStates currentState;
    private Vector3 roamPosition;

    private float lastRepositionTime;
    private float lastAttackTime;

    private bool isAttacking;

    private enum EnemyStates { Roaming, Following }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentState = EnemyStates.Roaming;
        roamPosition = GenerateRoamPosition();
    }

    private void Update()
    {
        if (player == null || enemyView == null || enemyView.Enemy == null)
            return;

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

        agent.isStopped = false;
        agent.speed = roamSpeed;
        agent.SetDestination(roamPosition);

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
            isAttacking = false;


            if (distance < minAttackDistance)
            {
                Vector3 dir = (transform.position - player.position).normalized;
                Vector3 target = player.position + dir * optimalDistance;

                MoveTo(target, retreatSpeed);
            }
            else if (distance > maxAttackDistance)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                Vector3 target = player.position - dir * optimalDistance;

                MoveTo(target, combatSpeed);
            }
            else
            {
                isAttacking = true;
                agent.isStopped = true;

                RotateTowardsPlayer();

                animator.Iswalking(false);
                animator.Isrunning(false);

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    enemyView.Enemy.Attack();
                    animator.PlayAttack();
                    lastAttackTime = Time.time;
                }
            }

            lastRepositionTime = Time.time;
        }

        if (!isAttacking)
            agent.isStopped = false;
    }

    private void MoveTo(Vector3 target, float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;

        SetDestination(target);

        animator.Iswalking(false);
        animator.Isrunning(true);
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
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
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
}