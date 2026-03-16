using UnityEngine;
using System;

[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public float currentHP;
    [HideInInspector] public float maxHP = 100f;
    [HideInInspector] public float physicalDamage = 10f;
    [HideInInspector] public float magicDamage = 25f;
    [HideInInspector] public string mk_Tag = "MK";
    [HideInInspector] public string mk_spear_Tag = "MK_spear";
    [HideInInspector] public string mk_magic_Tag = "MK_magic";

    private HealthComp _health;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public void Initialize(HealthComp health)
    {
        _health = health;
        _health.OnHealthChanged += (curr, max) => {
            currentHP = curr;
            maxHP = max;
            OnHealthChanged?.Invoke(curr, max);
        };
        _health.OnDeath += () => {
            currentHP = 0;
            OnDeath?.Invoke();
        };
        currentHP = _health.CurHP;
        maxHP = _health.MaxHP;
    }

    public float CurrentHP => _health?.CurHP ?? currentHP;
    public float MaxHP => _health?.MaxHP ?? maxHP;
    public bool IsAlive => _health?.IsAlive ?? (currentHP > 0);
    public void TakeDamage(float damage) => _health?.TakeDamage(damage);

    public void ResetStats() => _health?.Reset();

    [System.Obsolete("Use TakeDamage() instead of modifying currentHP directly")]
    public void SetCurrentHP(float value)
    {
        if (_health != null) _health.SetHP(value);
        else currentHP = value;
    }
}