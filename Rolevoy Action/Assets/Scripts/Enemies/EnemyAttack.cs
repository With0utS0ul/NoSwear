using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float coolDown = 1f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;
    [SerializeField] private Collider attackTrigger;

    private float timer;
    public bool CanAttack { get; private set; } = true;
    public float AttackRange => attackRange;

    private void Start()
    {
        if (attackTrigger != null)
        {
            attackTrigger.isTrigger = true;
            attackTrigger.enabled = false;
        }
    }

    private void Update()
    {
        if (!CanAttack)
        {
            timer += Time.deltaTime;
            if (timer >= coolDown)
            {
                CanAttack = true;
                timer = 0;
            }
        }
    }

    public void TryAttackPlayer()
    {
        if (!CanAttack) return;
        if (attackTrigger != null)
            StartCoroutine(EnableTriggerForFrame());
        CanAttack = false;
    }

    private IEnumerator EnableTriggerForFrame()
    {
        attackTrigger.enabled = true;
        yield return new WaitForSeconds(0.2f);
        attackTrigger.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackTrigger.enabled) return;
        if (other.CompareTag("Player"))
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