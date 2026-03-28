using UnityEngine;
using System;

public class Health : IHealth
{
    public float Current { get; private set; }
    public float Max { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    public Health(float max)
    {
        Max = max;
        Current = max;
    }

    public void Take(float value)
    {
        Current -= value;
        if (Current < 0) Current = 0;
        OnHealthChanged?.Invoke(Current);
        if (Current <= 0)
            OnDeath?.Invoke();
    }

    public void Heal(float value)
    {
        Current = Mathf.Min(Current + value, Max);
        OnHealthChanged?.Invoke(Current);
    }

    public void Restore(float value)
    {
        Current = Mathf.Clamp(value, 0, Max);
        OnHealthChanged?.Invoke(Current);
    }
}