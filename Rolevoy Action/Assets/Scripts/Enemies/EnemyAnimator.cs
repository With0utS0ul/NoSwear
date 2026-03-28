using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Run = Animator.StringToHash("Isrunning");
    private static readonly int Walk = Animator.StringToHash("Iswalking");
    private static readonly int GetDamage = Animator.StringToHash("GetDamage");
    private static readonly int Death = Animator.StringToHash("Death");

    public void PlayAttack() => animator.SetTrigger(Attack);
    public void Isrunning(bool condition) => animator.SetBool(Run, condition);
    public void Iswalking(bool condition) => animator.SetBool(Walk, condition);
    public void SetGetDamage() => animator.SetTrigger(GetDamage);
    public void SetDeath() => animator.SetTrigger(Death);
}