using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float damage = 15f;
    [SerializeField] private DamageType damageType = DamageType.Physical;
    [SerializeField] private Collider triggerCollider;

    private Player owner;

    private void Awake()
    {
        if (triggerCollider == null) triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
        triggerCollider.enabled = false;
    }

    public void SetOwner(Player player) => owner = player;

    public void Attack() => StartCoroutine(EnableTriggerForFrame());

    private IEnumerator EnableTriggerForFrame()
    {
        triggerCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        triggerCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCollider.enabled) return;
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damage dmg = new Damage();
                if (damageType == DamageType.Physical) dmg.Physical = damage;
                else dmg.Magical = damage;
                damageable.ApplyDamage(dmg);
            }
        }
    }
}