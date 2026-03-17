using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    //[SerializeField] private float _damage = 10f;
    [SerializeField] private float _lifetime = 3f; // Время жизни снаряда

    private Transform _target;

    public void Initialize(Transform target)
    {
        _target = target;
        Destroy(gameObject, _lifetime); // Автоудаление через 3 секунды
    }

    private void Update()
    {
        if (_target != null)
        {
            // Летим к цели
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            // Поворачиваем снаряд в направлении полета
            if (direction != Vector3.zero)
            {
                transform.LookAt(_target);
            }
        }
        else
        {
            // Если цели нет - летим прямо
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем столкновение с игроком
        if (other.CompareTag("MK"))
        {

            // Уничтожаем снаряд
            Destroy(gameObject);
        }
    }
}