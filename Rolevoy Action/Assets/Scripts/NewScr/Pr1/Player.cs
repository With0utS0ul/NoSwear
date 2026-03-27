using UnityEngine;

public class Player : Entity
{
    private IDamageService damageService;

    public Player(IHealth health, IDamageService damageService)
    {
        this.health = health;
        this.damageService = damageService;
    }

    public void Attack(IDamageable target)
    {
        damageService.DealDamage(target, new Damage
        {
            Physical = 10
        });
    }
}