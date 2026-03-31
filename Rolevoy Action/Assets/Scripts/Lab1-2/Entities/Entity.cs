using System;

public abstract class Entity : IDamageable
{
    protected IHealth health;

    public bool IsAlive => health.Current > 0;
    public IHealth Health => health;

    public event Action OnDeath;
    public event Action OnDamage;

    public virtual void ApplyDamage(Damage damage)
    {
        OnDamage?.Invoke();
        float total = damage.Physical + damage.Magical;
        health.Take(total);
        
        if (!IsAlive)
            OnDeath?.Invoke();

        
    }
}