using UnityEngine;
using System;

public class HealthComp : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;

    private Health health;

    public float Current => health.Current;
    public float Max => health.Max;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        health = new Health(maxHealth);
        health.OnHealthChanged += h => OnHealthChanged?.Invoke(h);
        health.OnDeath += () => OnDeath?.Invoke();
    }

    public void Take(float value) => health.Take(value);
    public void Heal(float value) => health.Heal(value);
    public void Restore(float value) => health.Restore(value);
}