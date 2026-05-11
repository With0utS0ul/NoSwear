using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "AI/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic")]
    public string weaponName;
    public bool isMelee = true;      // true Ц ближнее, false Ц дальнее
    public float damage = 10f;
    public float cooldown = 1.5f;
    public float range = 2f;          // дл€ ближнего Ц дистанци€ атаки, дл€ дальнего Ц оптимальна€ дистанци€
    public float optimalDistance = 7f; // дл€ дальнего бо€

    [Header("Effects")]
    public GameObject hitEffectPrefab;   // эффект при попадании
    public AudioClip attackSound;
    public GameObject muzzleFlashPrefab;  // дл€ дальнего оружи€
}