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
        if (!IsAlive)
        {   
            OnDeath?.Invoke(); 
            return; 
        }

        OnDamage?.Invoke();
        float total = damage.Physical + damage.Magical;
        health.Take(total);

        
        




    }
}