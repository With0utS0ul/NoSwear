using UnityEngine;

public class EntityView : MonoBehaviour, IDamageable
{
    private Entity entity;

    public void Init(Entity entity)
    {
        this.entity = entity;
    }

    public void ApplyDamage(Damage damage)
    {
        entity.ApplyDamage(damage);
        if (!entity.IsAlive)
            Die();
    }

    private void Die()
    {
        // Можно вызвать анимацию смерти, если есть
        Destroy(gameObject);
    }
}