using UnityEngine;

public class MagAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    
    [SerializeField] private float _coolDown = 2f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

   
    public float CoolDown => _coolDown;
    public bool CanAttack { get; private set; } = true;

    private float _lastAttackTime;

    private void Update()
    {
        if (Time.time - _lastAttackTime >= _coolDown)
        {
            CanAttack = true;
        }
    }

    public void TryAttackPlayer(Transform playerTransform)
    {
        if (!CanAttack) return;

        if (_projectilePrefab != null && _firePoint != null)
        {
            GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
            MagicProjectile magicProjectile = projectile.GetComponent<MagicProjectile>();
            if (magicProjectile != null)
            {
                magicProjectile.Initialize(playerTransform);
            }
        }

        CanAttack = false;
        _lastAttackTime = Time.time;
    }

}