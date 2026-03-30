public class MeleeAttack : IAttack
{
    private MeleeWeapon weapon;

    public MeleeAttack(MeleeWeapon weapon)
    {
        this.weapon = weapon;
    }

    public void Execute()
    {
        weapon.Attack();
    }
}