using UnityEngine;

public enum BossElementType { Sand, Sun, Nile, Dark }
public enum BossWeaponType { Melee, Ranged }

[CreateAssetMenu(fileName = "BossWeapon", menuName = "AI/Boss Weapon Profile")]
public class BossWeaponProfile : ScriptableObject
{
    public string profileName;
    public BossWeaponType weaponType;
    public BossElementType element;

    [Header("Weapon Model Material")]
    public Material weaponMaterial;

    
    [Header("Boss Body Material")]
    public Material bodyMaterial;   // если не null, будет применён к модели босса
    

    [Header("Attack Visuals & Audio")]
    public GameObject attackVfxPrefab;
    public AudioClip attackSound;
    public Color projectileColor = Color.white;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;

    [Header("Gameplay Modifiers")]
    public float damageMultiplier = 1f;
    public float cooldownMultiplier = 1f;
}