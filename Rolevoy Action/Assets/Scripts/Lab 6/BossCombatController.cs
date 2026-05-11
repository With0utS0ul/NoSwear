using UnityEngine;

public class BossCombatController : MonoBehaviour
{
    [SerializeField] private BossWeaponProfile[] availableProfiles;
    public BossWeaponProfile CurrentProfile { get; private set; }

    private Renderer weaponRenderer;  // меч/оружие
    private Renderer shieldRenderer;  // щит (опционально)

    private MagAttack magAttack;
    private EnemyView enemyView;

    private void Awake()
    {
        enemyView = GetComponent<EnemyView>();
        magAttack = GetComponent<MagAttack>();

        // Поиск меча (по наличию "weapon" в имени)
        weaponRenderer = FindRendererByName("weapon");

        // Поиск щита (по наличию "shield" в имени) – опционально
        shieldRenderer = FindRendererByName("shield");

        if (weaponRenderer == null)
            Debug.LogWarning($"[{name}] Weapon not found. Rename child object to contain 'weapon'.");
        if (shieldRenderer == null)
            Debug.Log($"[{name}] No shield found (optional).");

        // Выбор случайного профиля
        if (availableProfiles != null && availableProfiles.Length > 0)
        {
            int idx = Random.Range(0, availableProfiles.Length);
            CurrentProfile = availableProfiles[idx];
            ApplyProfile();
        }
    }

    /// <summary>Ищет дочерний объект с именем, содержащим указанную строку, и возвращает его Renderer.</summary>
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

        Material newMat = Instantiate(CurrentProfile.weaponMaterial);

        // Применяем к оружию
        if (weaponRenderer != null)
        {
            ApplyMaterialToRenderer(weaponRenderer, newMat);
            Debug.Log($"Weapon material changed to {CurrentProfile.weaponMaterial.name}");
        }

        // Применяем к щиту, если он есть
        if (shieldRenderer != null)
        {
            ApplyMaterialToRenderer(shieldRenderer, newMat);
            Debug.Log($"Shield material changed to {CurrentProfile.weaponMaterial.name}");
        }

        // Настройка дальнего боя
        if (magAttack != null && CurrentProfile.projectilePrefab != null)
            magAttack.ApplyBossProfile(CurrentProfile);
    }

    /// <summary>Заменяет все слоты материалов на указанный материал.</summary>
    private void ApplyMaterialToRenderer(Renderer renderer, Material material)
    {
        int slotCount = renderer.sharedMaterials.Length;
        Material[] newMaterials = new Material[slotCount];
        for (int i = 0; i < slotCount; i++)
            newMaterials[i] = material;
        renderer.materials = newMaterials;
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
}