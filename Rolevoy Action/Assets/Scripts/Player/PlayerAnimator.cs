using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int AttackMelee = Animator.StringToHash("AttackMelee");
    private static readonly int AttackMagic = Animator.StringToHash("AttackMagic");
    private static readonly int GetDamage = Animator.StringToHash("GetDamage");
    private static readonly int Death = Animator.StringToHash("Death");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeed(float speed) => animator.SetFloat(Speed, speed);
    public void SetAttackMelee() => animator.SetTrigger(AttackMelee);
    public void SetAttackMagic() => animator.SetTrigger(AttackMagic);
    public void SetGetDamage() => animator.SetTrigger(GetDamage);
    public void SetDeath() => animator.SetTrigger(Death);
}