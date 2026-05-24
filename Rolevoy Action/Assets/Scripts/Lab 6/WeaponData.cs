using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "AI/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic")]
    public string weaponName;
    public bool isMelee = true;      
    public float damage = 10f;
    public float cooldown = 1.5f;
    public float range = 2f;          
    public float optimalDistance = 7f; 

    [Header("Effects")]
    public GameObject hitEffectPrefab;   
    public AudioClip attackSound;
    public GameObject muzzleFlashPrefab;
}