using UnityEngine;
using System;

public class Health : IHealth
{
    private bool isDead = false;
    public float Current { get; private set; }
    public float Max { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnDamage;

    public Health(float max)
    {
        Max = max;
        Current = max;
    }

    public void Take(float value)
    {

        if (isDead || value <= 0) return;

        OnDamage?.Invoke();

        Current = Mathf.Max(Current - value, 0);
        OnHealthChanged?.Invoke(Current);

        if (Current <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float value)
    {
        if (isDead || value <= 0) return; 
        Current = Mathf.Min(Current + value, Max);
        OnHealthChanged?.Invoke(Current);
    }

    public void Restore(float value)
    {
        Current = Mathf.Clamp(value, 0, Max);
        OnHealthChanged?.Invoke(Current);
    }
}