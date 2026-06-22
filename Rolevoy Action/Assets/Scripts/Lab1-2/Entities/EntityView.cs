using UnityEngine;
using System.Collections;

public class EntityView : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] private float damageDelay = 0.5f;

    private Entity entity;
    private bool isDead;

    public void Init(Entity entity)
    {
        this.entity = entity;

        entity.OnDeath += OnDeath;
        entity.OnDamage += OnDamage;
    }

    public void ApplyDamage(Damage damage)
    {
        if (isDead) return;

        entity.ApplyDamage(damage);
    }

    private void OnDeath()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DeathRoutine());
    }

    private void OnDamage()
    {
        if (isDead) return;

        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        if (animator != null)
            animator.SetTrigger("Death");
        var controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;
        yield return new WaitForSeconds(deathDelay);
    }

    private IEnumerator DamageRoutine()
    {
        if (animator != null)
            animator.SetTrigger("GetDamage");
        var controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;
        yield return new WaitForSeconds(damageDelay);

        if (controller != null)
            controller.enabled = true;
    }

    private void OnDestroy()
    {
        if (entity != null)
        {
            entity.OnDeath -= OnDeath;
            entity.OnDamage -= OnDamage;
        }
        
    }
}