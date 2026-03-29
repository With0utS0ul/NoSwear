public class Enemy : Entity
{
    private IAttack attack;

    public Enemy(IHealth health, IAttack attack)
    {
        this.health = health;
        this.attack = attack;
    }

    public void Attack()
    {
        attack?.Execute();
    }
}