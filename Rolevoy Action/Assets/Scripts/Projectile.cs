using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   

    [Header("Настройки снаряда")]
    public float speed = 20f;
    public float lifetime = 3f;
    public GameObject hitEffect;

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

        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // самого игрока
        if (other.CompareTag("MK")) return;

        hasHit = true;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }


    
}

