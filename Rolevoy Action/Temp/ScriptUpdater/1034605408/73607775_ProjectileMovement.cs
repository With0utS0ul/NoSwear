using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("⚙️ Настройки")]
    [SerializeField] private float _lifeTime = 5f;        // Время жизни снаряда
    [SerializeField] private LayerMask _hitLayers;        // Слои, с которыми снаряд взаимодействует

    private Vector3 _direction;
    private float _speed;
    private float _damage;
    private Transform _target;
    private float _timer;
    private bool _hasHit = false;

    /// <summary>
    /// Инициализация снаряда
    /// </summary>
    public void Initialize(Vector3 direction, float speed, float damage, Transform target)
    {
        _direction = direction;
        _speed = speed;
        _damage = damage;
        _target = target;
        _timer = 0f;
        _hasHit = false;
    }

    private void Update()
    {
        if (_hasHit) return;

        _timer += Time.deltaTime;

        // Уничтожить по таймеру
        if (_timer >= _lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // Движение вперёд
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

        // Опционально: слежение за игроком (плавная коррекция курса)
        if (_target != null)
        {
            Vector3 predictedPos = _target.position + GetComponent<Rigidbody>()?.linearVelocity * 0.5f ?? Vector3.zero;
            Vector3 newDirection = (predictedPos - transform.position).normalized;
            _direction = Vector3.Lerp(_direction, newDirection, 0.02f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;

        // Проверка: попали ли в игрока
        if (other.TryGetComponent<Movement>(out Movement player))
        {
            // 🎯 Наносим урон (адаптируйте под вашу систему урона)
            // Пример: player.TakeDamage(_damage);
            Debug.Log($"[Projectile] Hit player for {_damage} damage!");

            _hasHit = true;
            Destroy(gameObject); // Уничтожаем снаряд
            return;
        }

        // Проверка: попали ли в препятствие
        if (((1 << other.gameObject.layer) & _hitLayers) != 0)
        {
            _hasHit = true;
            Destroy(gameObject);
        }
    }
}