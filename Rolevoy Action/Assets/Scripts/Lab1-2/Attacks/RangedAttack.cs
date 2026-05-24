using UnityEngine;

public class RangedAttack : IAttack
{
    private Transform firePoint;
    private GameObject projectilePrefab;
    private float damage;
    private float speed;

    public RangedAttack(Transform firePoint, GameObject prefab, float damage, float speed)
    {
        this.firePoint = firePoint;
        this.projectilePrefab = prefab;
        this.damage = damage;
        this.speed = speed;
    }

    public void Execute()
    {
        var obj = Object.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        var projectile = obj.GetComponent<Projectile>();
        projectile?.Init(firePoint.forward, damage, DamageType.Magical, speed);
    }
}