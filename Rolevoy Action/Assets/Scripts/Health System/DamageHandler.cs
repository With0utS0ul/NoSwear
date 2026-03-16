using UnityEngine;

[RequireComponent(typeof(HealthComp))]
public class DamageHandler : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private string attackerTagToIgnore = "";

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Stats for Legacy Compatibility")]
    [HideInInspector] public float physicalDamage = 10f;
    [HideInInspector] public float magicDamage = 25f;
    [HideInInspector] public string characterTag = "Default";

    private HealthComp health;

    private void Awake()
    {
        health = GetComponent<HealthComp>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject ||
            other.transform.IsChildOf(transform) ||
            transform.IsChildOf(other.transform))
            return;

        if (health == null || !health.IsAlive) return;

        var attackerHealth = other.GetComponentInParent<HealthComp>();
        if (attackerHealth != null && !attackerHealth.IsAlive)
            return;

        float damage = GetDamageFromAttacker(other);
        if (damage <= 0) return;

        health.TakeDamage(damage);
        PlayHitAnimation();
    }

    private float GetDamageFromAttacker(Collider attacker)
    {
        return GetDamageByTag(attacker.CompareTag);
    }

    private float GetDamageByTag(System.Func<string, bool> hasTag)
    {
        if (hasTag("MK_spear")) return 10f; 
        if (hasTag("MK_magic")) return 25f;
        if (hasTag("Spider")) return 10f; 
        if (hasTag("Maga")) return 25f;
        return 0f;
    }

    private void PlayHitAnimation()
    {
        if (animator != null && health.IsAlive)
        {
            animator.SetTrigger("Hit");
        }
    }
    private void OnEnable() => health.OnDeath += OnCharacterDeath;
    private void OnDisable() => health.OnDeath -= OnCharacterDeath;

    private void OnCharacterDeath()
    {
        if (animator != null) animator.SetTrigger("Death");
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        Destroy(gameObject, 0.5f); 
    }
}