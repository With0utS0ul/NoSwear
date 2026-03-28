using UnityEngine;

public class Player : Entity
{
    private IDamageService damageService;

    public int MeleeDamage = 15;
    public int MagicDamage = 20;

    private MeleeWeapon meleeWeapon;
    private Transform firePoint;
    private GameObject projectilePrefab;

    public Player(IHealth health, IDamageService damageService)
    {
        this.health = health;
        this.damageService = damageService;
    }

    public void SetMeleeWeapon(MeleeWeapon weapon) => meleeWeapon = weapon;
    public void SetMagicAttack(Transform firePoint, GameObject projectilePrefab)
    {
        this.firePoint = firePoint;
        this.projectilePrefab = projectilePrefab;
    }

    public void AttackMelee() => meleeWeapon?.Attack();
    public void AttackMagic()
    {
        if (projectilePrefab == null || firePoint == null) return;
        var projectileObj = Object.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        var projectile = projectileObj.GetComponent<Projectile>();
        projectile?.Init(firePoint.forward, MagicDamage, DamageType.Magical);
    }
}