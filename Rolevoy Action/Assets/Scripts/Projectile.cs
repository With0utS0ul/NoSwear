using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   

    [Header("Ќастройки снар€да")]
    public float speed = 20f;
    public float lifetime = 3f; // ¬рем€ жизни снар€да
    public float damage = 10f;
    public GameObject hitEffect; // Ёффект при попадании (опционально)

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

        // ƒвижение снар€да
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // »гнорируем триггеры и самого игрока
        if (other.isTrigger || other.CompareTag("MK")) return;

        hasHit = true;


        // Ёффект попадани€
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // ”ничтожение снар€да
        Destroy(gameObject);
    }


    
}

