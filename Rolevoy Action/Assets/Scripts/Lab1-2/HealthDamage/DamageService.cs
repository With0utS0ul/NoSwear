using UnityEngine;

public class DamageService : IDamageService
{
    public void DealDamage(IDamageable target, Damage damage)
    {
        if (target == null) return;

        target.ApplyDamage(damage);
    }
}