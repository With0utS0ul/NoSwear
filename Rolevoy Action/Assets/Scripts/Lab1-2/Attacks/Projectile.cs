using UnityEngine;

public enum DamageType { Physical, Magical }

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;

    private float damage;
    private DamageType damageType;
    private Vector3 direction;

    private IDamageService _damageService;

    public void Init(Vector3 dir, float dmg, DamageType type, float speed) 
    {
        direction = dir.normalized;
        damage = dmg;
        damageType = type;

        _damageService = GameEntryPoint.Instance?.DamageService;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        string targetTag = (gameObject.CompareTag("PlayerProjectile")) ? "Enemy" : "Player";
        if (other.CompareTag(targetTag))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damage dmg = new Damage();
                if (damageType == DamageType.Physical) dmg.Physical = damage;
                else dmg.Magical = damage;
                //damageable.ApplyDamage(dmg);
                _damageService.DealDamage(damageable, dmg);
            }
            Destroy(gameObject);
        }
    }
}