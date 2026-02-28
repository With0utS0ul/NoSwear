/*using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _minWalkableDistance;
    [SerializeField] private float _maxWalkableDistance;
    [SerializeField] private float _reachedPointDistance;
    [SerializeField] private GameObject _roamTarget;
    [SerializeField] private float _targetFollowRange;
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private float _stopTargetFollowingRange;
    [SerializeField] private AIDestinationSetter _aiDestinationSetter;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private AIPath _aiPath;
    private Movement _player;
    private EnemyStates _currentState;
    private Vector3 _roamPosition;
    private void Start()
    {
        _player = FindObjectOfType<Movement>();
        _currentState = EnemyStates.Roaming;
        _roamPosition = GenerateRoamPosition();
    }
    private void Update()
    {
        switch (_currentState)
        {
            case EnemyStates.Roaming:
                _roamTarget.transform.position = _roamPosition;

                if

                (Vector3.Distance(gameObject.transform.position, _roamPosition) <= _reachedPointDistance)
                {
                    _roamPosition = GenerateRoamPosition();
                }

                _aiDestinationSetter.target = _roamTarget.transform;
                TryFindPlayer();
                _enemyAnimator.Iswalking(true);
                _enemyAnimator.Isrunning(false);
                _aiPath.maxSpeed = 3;
                break;
            case EnemyStates.Following:
                _aiDestinationSetter.target = _player.transform;
                _enemyAnimator.Iswalking(false);
                _enemyAnimator.Isrunning(true);
                _aiPath.maxSpeed = 3;
                if
                (Vector3.Distance(gameObject.transform.position, _player.transform.position) < _enemyAttack.AttackRange)
                {
                    _enemyAnimator.Iswalking(false);
                    _enemyAnimator.Isrunning(false);
                    if (_enemyAttack.CanAttack)
                    {
                        _enemyAttack.TryAttackPlayer();
                        _enemyAnimator.PlayAttack();
                    }


                }
                if

                (Vector3.Distance(gameObject.transform.position, _player.transform.position) >= _stopTargetFollowingRange)
                {
                    _currentState = EnemyStates.Roaming;
                }

                break;

        }
    }
    private void TryFindPlayer()
    {
        if (Vector3.Distance(gameObject.transform.position, _player.transform.position) <= _targetFollowRange)
        {
            _currentState = EnemyStates.Following;
        }
    }
    private Vector3 GenerateRoamPosition()
    {
        var _roamPosition = gameObject.transform.position + GenerateRandomDirection() * GenerateRandomWalkableDistance();
        return _roamPosition;
    }
    private float GenerateRandomWalkableDistance()
    {
        var randomDistance = Random.Range(_minWalkableDistance, _maxWalkableDistance);
        return randomDistance;
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


*/