using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerView playerView;

    public void ApplyDamage(Damage damage)
    {
        playerView.Player.ApplyDamage(damage);
    }
}