using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    
    [SerializeField] public float coolDown;
    private float timer;
    public bool CanAttack { get; private set; }
    private Movement player;
    public float AttackRange => _attackRange;

    [System.Obsolete]
    private void Start()
    {
        player = FindObjectOfType<Movement>();

    }
    private void Update()
    {
        UpdatecoolDown();
    }
    public void UpdatecoolDown()
    {

        if (CanAttack)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer < coolDown) return;
        CanAttack = true;
        timer = 0;
    }
    public void TryAttackPlayer()
    {
        
        CanAttack = false;
    }


}
