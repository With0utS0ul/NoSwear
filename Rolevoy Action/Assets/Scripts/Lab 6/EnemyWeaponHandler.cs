using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;
    private float lastAttackTime;

    public WeaponData CurrentWeapon => currentWeapon;

    public void SetWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;
    }

    public bool CanAttack => Time.time >= lastAttackTime + currentWeapon.cooldown;

    public void PerformAttack(EnemyContext context, Transform target)
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;

        // Âîñïðîèçâåäåíèå çâóêà
        if (currentWeapon.attackSound != null)
            AudioSource.PlayClipAtPoint(currentWeapon.attackSound, context.transform.position);

        // Ýôôåêò äóëüíîãî âñïëåñêà äëÿ äàëüíåãî îðóæèÿ
        if (!currentWeapon.isMelee && currentWeapon.muzzleFlashPrefab != null)
            Instantiate(currentWeapon.muzzleFlashPrefab, context.transform.position, Quaternion.identity);

        // Íàíåñåíèå óðîíà (ïðèìåð – ÷åðåç EnemyView)
        if (target != null && context.enemyView != null && context.enemyView.Enemy != null)
        {
            // Çäåñü âûçûâàåòñÿ ñóùåñòâóþùàÿ ñèñòåìà àòàêè âðàãà
            context.enemyView.Enemy.Attack();
        }

        
    }

    // Äëÿ ïðîâåðêè òèïà îðóæèÿ â ChaseState
    public bool HasRangedWeapon => !currentWeapon.isMelee;
    public float GetOptimalDistance => currentWeapon.optimalDistance;
    public float GetAttackRange => currentWeapon.range;
} 
