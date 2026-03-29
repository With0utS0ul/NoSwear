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
        Debug.Log("HIT: " + other.name);
        if (!triggerCollider.enabled) return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("DAMAGE APPLIED");
            Damage dmg = new Damage();

            if (damageType == DamageType.Physical)
                dmg.Physical = damage;
            else
                dmg.Magical = damage;

            damageable.ApplyDamage(dmg);
        }
        if (damageable == null)
        {
            Debug.Log("NO DAMAGEABLE FOUND!");
            return;
        }
    }
}