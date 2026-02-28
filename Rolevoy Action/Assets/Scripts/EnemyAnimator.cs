using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private static readonly int Attack = Animator.StringToHash(name: "Attack");
    private static readonly int Run = Animator.StringToHash(name: "Isrunning");
    private static readonly int Walk = Animator.StringToHash(name: "Iswalking");
    public void PlayAttack()
    {
        _animator.SetTrigger(id: Attack);

    }
    public void Isrunning(bool condition)
    {
        _animator.SetBool(id: Run, condition);
    }
    public void Iswalking(bool condition)
    {
        _animator.SetBool(id: Walk, condition);
    }


}
