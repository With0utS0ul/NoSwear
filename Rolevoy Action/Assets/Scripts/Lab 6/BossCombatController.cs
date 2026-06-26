using UnityEngine;

public class BossCombatController : MonoBehaviour
{
    [SerializeField] private BossWeaponProfile[] availableProfiles;
    public BossWeaponProfile CurrentProfile { get; private set; }

    private Renderer weaponRenderer;
    private Renderer shieldRenderer;
    
    private Renderer bodyRenderer;
    

    private MagAttack magAttack;
    private EnemyView enemyView;

    private void Awake()
    {
        enemyView = GetComponent<EnemyView>();
        magAttack = GetComponent<MagAttack>();

        weaponRenderer = FindRendererByName("weapon");
        shieldRenderer = FindRendererByName("shield");
        
        bodyRenderer = FindRendererByName("body");
        if (bodyRenderer == null)
            Debug.LogWarning($"[{name}] Boss body renderer not found. Create a child object with 'body' in its name.");
        

        if (weaponRenderer == null)
            Debug.LogWarning($"[{name}] Weapon not found. Rename child object to contain 'weapon'.");
        if (shieldRenderer == null)
            Debug.Log($"[{name}] No shield found (optional).");

        if (availableProfiles != null && availableProfiles.Length > 0)
        {
            int idx = Random.Range(0, availableProfiles.Length);
            CurrentProfile = availableProfiles[idx];
            ApplyProfile();
        }
    }

    private Renderer FindRendererByName(string namePart)
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.name.ToLower().Contains(namePart.ToLower()))
            {
                Renderer rend = child.GetComponent<Renderer>();
                if (rend != null) return rend;
            }
        }
        return null;
    }

    private void ApplyProfile()
    {
        if (CurrentProfile == null || CurrentProfile.weaponMaterial == null)
        {
            Debug.LogWarning("Cannot apply profile: missing profile or material");
            return;
        }
        Material newWeaponMat = Instantiate(CurrentProfile.weaponMaterial);
        if (weaponRenderer != null)
        {
            ApplyMaterialToRenderer(weaponRenderer, newWeaponMat);
            Debug.Log($"Weapon material changed to {CurrentProfile.weaponMaterial.name}");
        }
        if (shieldRenderer != null)
        {
            ApplyMaterialToRenderer(shieldRenderer, newWeaponMat);
            Debug.Log($"Shield material changed to {CurrentProfile.weaponMaterial.name}");
        }

        
        if (bodyRenderer != null && CurrentProfile.bodyMaterial != null)
        {
            Material newBodyMat = Instantiate(CurrentProfile.bodyMaterial);
            ApplyMaterialToRenderer(bodyRenderer, newBodyMat);
            Debug.Log($"Body material changed to {CurrentProfile.bodyMaterial.name}");
        }
        
        if (magAttack != null && CurrentProfile.projectilePrefab != null)
            magAttack.ApplyBossProfile(CurrentProfile);
    }

    private void ApplyMaterialToRenderer(Renderer renderer, Material material)
    {
        int slotCount = renderer.sharedMaterials.Length;
        Material[] newMaterials = new Material[slotCount];
        for (int i = 0; i < slotCount; i++)
            newMaterials[i] = material;
        renderer.materials = newMaterials;
    }

    public bool CanDoRangedAttack()
    {
        return magAttack != null && magAttack.CanAttack;
    }

    public void PerformAttack(Transform target)
    {
        if (CurrentProfile == null) return;
        if (CurrentProfile.attackSound != null)
            AudioSource.PlayClipAtPoint(CurrentProfile.attackSound, transform.position);
        if (CurrentProfile.attackVfxPrefab != null)
            Instantiate(CurrentProfile.attackVfxPrefab, transform.position + transform.forward, Quaternion.identity);
        enemyView?.Enemy?.Attack();
    }

    public void PerformRangedAttack(Transform target)
    {
        if (CurrentProfile == null || CurrentProfile.weaponType != BossWeaponType.Ranged)
            return;
        if (!CanDoRangedAttack())
            return;

        if (CurrentProfile.attackSound != null)
            AudioSource.PlayClipAtPoint(CurrentProfile.attackSound, transform.position);
        if (CurrentProfile.attackVfxPrefab != null)
            Instantiate(CurrentProfile.attackVfxPrefab, transform.position + transform.forward, Quaternion.identity);

        magAttack.TryAttackPlayer(target);
    }
}