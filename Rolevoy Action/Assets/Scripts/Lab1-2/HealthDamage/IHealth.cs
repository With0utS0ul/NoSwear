using System;

public interface IHealth
{
    float Current { get; }
    float Max { get; }

    event Action<float> OnHealthChanged;
    event Action OnDeath;
    event Action OnDamage;

    void Take(float value);
    void Heal(float value);
    void Restore(float value);
}