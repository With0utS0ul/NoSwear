using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;

    public void Init(Player player)
    {
        this.player = player;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
                player.Attack(damageable);
        }
    }
}