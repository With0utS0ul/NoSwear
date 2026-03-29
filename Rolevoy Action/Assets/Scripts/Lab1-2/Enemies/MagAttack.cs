using UnityEngine;

public class MagAttack : MonoBehaviour
{
    [SerializeField] private float coolDown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public bool CanAttack { get; private set; } = true;
    private float lastAttackTime;

    private void Update()
    {
        if (Time.time - lastAttackTime >= coolDown)
            CanAttack = true;
    }

    public void TryAttackPlayer(Transform playerTransform)
    {
        if (!CanAttack) return;
        if (projectilePrefab != null && firePoint != null)
        {
            var projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            var projectile = projectileObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector3 direction = (playerTransform.position - firePoint.position).normalized;
                projectile.Init(direction, 20f, DamageType.Magical);
                projectile.gameObject.tag = "EnemyProjectile"; // важно для идентификации
            }
        }
        CanAttack = false;
        lastAttackTime = Time.time;
    }
}