using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "AI/Attack Profile")]
public class AttackProfile : ScriptableObject
{
    public string profileName;
    public bool isMelee = true;
    public float damage = 10f;        // можно использовать дл€ изменени€ урона
    public float cooldown = 1.5f;     // опционально
    // public float range;             // ”ƒјЋ»“№
    // public float optimalDistance;   // ”ƒјЋ»“№

    [Header("Effects")]
    public GameObject attackVfxPrefab;
    public GameObject hitVfxPrefab;
    public AudioClip attackSound;

    [Header("Ranged specific")]
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float projectileSpeed = 10f;
}