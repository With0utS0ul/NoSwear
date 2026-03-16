using UnityEngine;
using System;

public class HealthComp : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHP = 100f;

    public float MaxHP => maxHP;

    private float currentHP;
    public float CurHP
    {
        get => currentHP;
        private set
        {
            if (Mathf.Approximately(currentHP, value)) return;

            currentHP = Mathf.Clamp(value, 0, maxHP);
            OnHealthChanged?.Invoke(currentHP, maxHP);
            if (currentHP <= 0 && !IsDead)
            {
                IsDead = true;
                OnDeath?.Invoke();
            }
        }
    }

    public bool IsAlive => CurHP > 0;
    public bool IsDead { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    protected virtual void Awake() => Reset();

    public virtual void Reset()
    {
        IsDead = false;
        CurHP = maxHP;
    }

    public virtual void TakeDamage(float damage)
    {
        if (!IsAlive || damage <= 0) return;
        CurHP -= damage;
    }

    public void SetHP(float value) => CurHP = value;
}