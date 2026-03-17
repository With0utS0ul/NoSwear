using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   

    [Header("Настройки снаряда")]
    public float speed = 20f;
    public float lifetime = 3f; // Время жизни снаряда
    //public float damage = 10f;
    public GameObject hitEffect; // Эффект при попадании (опционально)

    private Vector3 direction;
    private bool hasHit = false;

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (hasHit) return;

        // Движение снаряда
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // самого игрока
        if (other.CompareTag("MK")) return;

        hasHit = true;


        // Эффект попадания
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Уничтожение снаряда
        Destroy(gameObject);
    }


    
}

