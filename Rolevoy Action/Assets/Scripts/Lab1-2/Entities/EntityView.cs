using UnityEngine;
using System.Collections;

public class EntityView : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private float deathDelay = 2f;

    private Entity entity;
    private bool isDead;

    public void Init(Entity entity)
    {
        this.entity = entity;

        entity.OnDeath += OnDeath;
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

    private IEnumerator DeathRoutine()
    {
        if (animator != null)
            animator.SetTrigger("Death");
        var controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;
        yield return new WaitForSeconds(deathDelay);
    }

    private void OnDestroy()
    {
        if (entity != null)
            entity.OnDeath -= OnDeath;
    }
}