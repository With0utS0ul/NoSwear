using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _attackRange;
    
    [SerializeField] private float coolDown;
    private float timer;
    public bool CanAttack { get; private set; }
    private Movement player;
    public float AttackRange => _attackRange;
    private void Start()
    {
        player = FindObjectOfType<Movement>();

    }
    private void Update()
    {
        UpdatecoolDown();
    }
    private void UpdatecoolDown()
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
