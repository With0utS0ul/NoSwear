using System.Collections.Generic;
using UnityEngine;
//lol
public class EnemiesRepository : IEnemiesRepository
{
    public void Save(GameObject[] enemies, GameData data)
    {
        data.EnemyPositions = new List<Vector3>();
        data.EnemyHealths = new List<float>();

        foreach (var enemy in enemies)
        {
            data.EnemyPositions.Add(enemy.transform.position);
            var enemyView = enemy.GetComponent<EnemyView>();
            float health = enemyView != null && enemyView.Enemy != null
                ? enemyView.Enemy.Health.Current
                : 100f;
            data.EnemyHealths.Add(health);
        }
    }

    public void Load(GameData data, GameObject[] enemies)
    {
        if (data.EnemyPositions == null || data.EnemyHealths == null) return;
        if (data.EnemyPositions.Count != enemies.Length) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].transform.position = data.EnemyPositions[i];
            var enemyView = enemies[i].GetComponent<EnemyView>();
            if (enemyView != null && enemyView.Enemy != null && i < data.EnemyHealths.Count)
            {
                enemyView.Enemy.Health.Restore(data.EnemyHealths[i]);
            }
        }
    }
}