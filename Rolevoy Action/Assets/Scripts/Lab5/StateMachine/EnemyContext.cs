// EnemyContext.cs – контекст, общий для всех состояний
using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public NavMeshAgent Agent;
    public Animator Animator;
    public Transform Player;
    public Enemy Enemy;
    public EnemyView View;          // ссылка на EnemyView (для доступа к атаке)
    public float AttackRange;
    public float ChaseRange;
    public float FleeRange = 15f;
    public float HealthThreshold = 0.3f; // 30% для бегства

    public EnemyContext(NavMeshAgent agent, Animator anim, Transform player, Enemy enemy, EnemyView view, float attackRange, float chaseRange)
    {
        Agent = agent;
        Animator = anim;
        Player = player;
        Enemy = enemy;
        View = view;
        AttackRange = attackRange;
        ChaseRange = chaseRange;
    }
}
