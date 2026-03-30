using UnityEngine;

public class DamageService : IDamageService
{
    public void DealDamage(IDamageable target, Damage damage)
    {
        target.ApplyDamage(damage);
    }
}