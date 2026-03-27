using UnityEngine;

public abstract class Entity
{
    protected IHealth health;

    public bool IsAlive => health.Current > 0;

    public virtual void ApplyDamage(Damage damage)
    {
        float total = damage.Physical + damage.Magical;
        health.Take(total);
    }
}