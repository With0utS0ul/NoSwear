using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "AI/Attack Profile")]
public class AttackProfile : ScriptableObject
{
    public string profileName;
    public bool isMelee = true;
    public float damage = 10f;
    public float cooldown = 1.5f;


    [Header("Combat Distances")]
    public float range = 2f;
    public float optimalDistance = 7f; // оптимальная дистанция для ranged


    [Header("Effects")]
    public GameObject attackVfxPrefab;
    public GameObject hitVfxPrefab;
    public AudioClip attackSound;

    [Header("Ranged specific")]
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float projectileSpeed = 10f;
}