using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMagAI : MonoBehaviour
{
    [Header("Roaming Settings")]
    [SerializeField] private float _minWalkableDistance = 2f;
    [SerializeField] private float _maxWalkableDistance = 10f;
    [SerializeField] private float _reachedPointDistance = 1f;
    [SerializeField] private GameObject _roamTarget;

    [Header("Detection Settings")]
    [SerializeField] private float _targetFollowRange = 15f;
    [SerializeField] private float _stopTargetFollowingRange = 20f;

    [Header("Ranged Combat Settings")]
    [SerializeField] private float _minAttackDistance = 5f;
    [SerializeField] private float _maxAttackDistance = 10f;
    [SerializeField] private float _optimalAttackDistance = 7f;
    [SerializeField] private float _repositionCooldown = 0.5f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Movement Settings")]
    [SerializeField] private float _roamSpeed = 2f;
    [SerializeField] private float _combatSpeed = 4f;
    [SerializeField] private float _retreatSpeed = 5f;
    [SerializeField] private float _angularSpeed = 720f;

    [Header("Components")]
    [SerializeField] private MagAttack _enemyAttack;
    [SerializeField] private EnemyAnimator _enemyAnimator;

    private NavMeshAgent _agent;
    private Movement _player;
    private EnemyStates _currentState;
    private Vector3 _roamPosition;
    private float _lastRepositionTime;
    private Vector3 _currentTargetPosition;
    private bool _hasTargetPosition;
    private bool _isRetreating;
    private bool _isAttacking;

    [System.Obsolete]
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<Movement>();

        _agent.speed = _roamSpeed;
        _agent.angularSpeed = _angularSpeed;
        _agent.stoppingDistance = 0.5f;
        _agent.acceleration = 15f;

        _currentState = EnemyStates.Roaming;
        _roamPosition = GenerateRoamPosition();
        _lastRepositionTime = Time.time;
        _isRetreating = false;
        _isAttacking = false;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyStates.Roaming:
                UpdateRoaming();
                break;
            case EnemyStates.Following:
                UpdateRangedCombat();
                break;
        }
    }

    private void UpdateRoaming()
    {
        _roamTarget.transform.position = _roamPosition;

        if (Vector3.Distance(transform.position, _roamPosition) <= _reachedPointDistance)
        {
            _roamPosition = GenerateRoamPosition();
        }

        _agent.destination = _roamPosition;
        _agent.speed = _roamSpeed;
        _agent.isStopped = false;

        TryFindPlayer();

        _enemyAnimator.Iswalking(true);
        _enemyAnimator.Isrunning(false);
    }

    private void UpdateRangedCombat()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer >= _stopTargetFollowingRange)
        {
            _currentState = EnemyStates.Roaming;
            _hasTargetPosition = false;
            _isRetreating = false;
            _isAttacking = false;
            return;
        }

        if (Time.time - _lastRepositionTime >= _repositionCooldown)
        {
            _hasTargetPosition = false;
        }

       
        // ЛОГИКА ОТСТУПЛЕНИЯ (игрок ближе 5м)
        
        if (distanceToPlayer < _minAttackDistance)
        {
            _isRetreating = true;
            _isAttacking = false;

            if (!_hasTargetPosition)
            {
                SetRetreatPosition();
                _hasTargetPosition = true;
                _lastRepositionTime = Time.time;
            }

            _agent.speed = _retreatSpeed;
            _agent.isStopped = false;

            _enemyAnimator.Iswalking(false);
            _enemyAnimator.Isrunning(true);

            return;
        }

        
        // ЛОГИКА АТАКИ (игрок на дистанции 5-10м)
        
        if (distanceToPlayer >= _minAttackDistance && distanceToPlayer <= _maxAttackDistance)
        {
            _isRetreating = false;
            _isAttacking = true;
            _hasTargetPosition = false;
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;

            //  ПОВОРАЧИВАЕМ К ИГРОКУ
            RotateTowardsPlayer();

            _enemyAnimator.Iswalking(false);
            _enemyAnimator.Isrunning(false);

            if (_enemyAttack.CanAttack)
            {
                _enemyAttack.TryAttackPlayer(_player.transform);
                _enemyAnimator.PlayAttack();
            }

            return;
        }

        
        // ЛОГИКА ПРИБЛИЖЕНИЯ (игрок дальше 10м)
        
        if (distanceToPlayer > _maxAttackDistance)
        {
            _isRetreating = false;
            _isAttacking = false;

            if (!_hasTargetPosition)
            {
                SetApproachPosition();
                _hasTargetPosition = true;
                _lastRepositionTime = Time.time;
            }

            _agent.speed = _combatSpeed;
            _agent.isStopped = false;

            _enemyAnimator.Iswalking(false);
            _enemyAnimator.Isrunning(true);
        }
    }

    // Поворот к игроку
    private void RotateTowardsPlayer()
    {
        if (_player == null) return;

        // Создаём направление к игроку (без учёта высоты Y)
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        directionToPlayer.y = 0;
        directionToPlayer.Normalize();

        if (directionToPlayer != Vector3.zero)
        {
            // Плавно поворачиваемся к игроку
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void SetRetreatPosition()
    {
        Vector3 retreatDirection = (transform.position - _player.transform.position).normalized;
        float retreatDistance = Random.Range(_optimalAttackDistance, _maxAttackDistance);
        _currentTargetPosition = _player.transform.position + retreatDirection * retreatDistance;

        if (NavMesh.SamplePosition(_currentTargetPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            _agent.destination = hit.position;
        }
        else
        {
            _agent.destination = transform.position + retreatDirection * 10f;
        }
    }

    private void SetApproachPosition()
    {
        Vector3 approachDirection = (_player.transform.position - transform.position).normalized;
        _currentTargetPosition = _player.transform.position - approachDirection * _optimalAttackDistance;

        if (NavMesh.SamplePosition(_currentTargetPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            _agent.destination = hit.position;
        }
    }

    private void TryFindPlayer()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _targetFollowRange)
        {
            _currentState = EnemyStates.Following;
        }
    }

    private Vector3 GenerateRoamPosition()
    {
        Vector3 randomPosition = transform.position + GenerateRandomDirection() * GenerateRandomWalkableDistance();

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    private float GenerateRandomWalkableDistance()
    {
        return Random.Range(_minWalkableDistance, _maxWalkableDistance);
    }

    private Vector3 GenerateRandomDirection()
    {
        var newDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        return newDirection.normalized;
    }

    public enum EnemyStates
    {
        Roaming,
        Following
    }
}