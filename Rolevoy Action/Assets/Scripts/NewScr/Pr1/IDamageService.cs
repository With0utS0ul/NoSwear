using UnityEngine;

public interface IDamageService
{
    void DealDamage(IDamageable target, Damage damage);
}