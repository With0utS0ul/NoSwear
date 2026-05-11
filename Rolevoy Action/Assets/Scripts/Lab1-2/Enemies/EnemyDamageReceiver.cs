using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyView enemyView;
    public void ApplyDamage(Damage damage)
    {
        enemyView.Enemy.ApplyDamage(damage);
        var ai = GetComponent<EnemyStateMachineAI>();
        if (ai != null) ai.OnDamageReceived(damage);
    }
}