using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    public event Action<int> OnScoreChanged;
    public event Action OnBossSpawned;
    public event Action OnVictory;

    private readonly int killsToSpawnBoss;
    private readonly int killsToWin;
    private readonly GameObject bossPrefab;
    private Transform bossSpawnPoint;

    private int currentKills = 0;
    private bool bossSpawned = false;
    private bool victoryTriggered = false;
    

    public ScoreManager(int killsToSpawnBoss, int killsToWin, GameObject bossPrefab, Transform bossSpawnPoint)
    {
        this.killsToSpawnBoss = killsToSpawnBoss;
        this.killsToWin = killsToWin;
        this.bossPrefab = bossPrefab;
        this.bossSpawnPoint = bossSpawnPoint;
    }

    public void RegisterEnemyDeath(EnemyView enemy, bool isBoss = false)
    {
        
        if (victoryTriggered) return;

        if (!isBoss)
        {
            currentKills++;
            OnScoreChanged?.Invoke(currentKills);

            if (!bossSpawned && currentKills >= killsToSpawnBoss)
                SpawnBoss();

            if (!victoryTriggered && currentKills >= killsToWin)
                TriggerVictory();
        }
        else
        {
            if (!victoryTriggered)
                TriggerVictory();
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            // Сохраняем ссылку на созданный объект босса
            GameObject bossObj = UnityEngine.Object.Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossSpawned = true;
            OnBossSpawned?.Invoke();
            Debug.Log("Boss spawned!");

            // Находим его EnemyView и подписываемся на его смерть
            EnemyView bossView = bossObj.GetComponent<EnemyView>();
            if (bossView != null)
            {
                bossView.OnDied += (deadBoss) =>
                {
                    // Вызываем регистрацию смерти для самого себя (true означает, что это босс)
                    RegisterEnemyDeath(deadBoss, isBoss: true);
                };
            }
        }
        else
        {
            Debug.LogError("Boss prefab or spawn point not set in ScoreManager!");
        }
    }

    public void UpdateSpawnPoint(Transform newSpawnPoint)
    {
        bossSpawnPoint = newSpawnPoint;
    }

    private void TriggerVictory()
    {
        if (victoryTriggered) return;
        victoryTriggered = true;

        var audio = GameEntryPoint.Instance?.AudioService;
        if (audio != null)
        {
            AudioClip victoryClip = Resources.Load<AudioClip>("Music/Victory");
            if (victoryClip != null)
                audio.PlayMusic(victoryClip);
            else
                Debug.LogWarning("Victory music clip not found!");
        }
        OnVictory?.Invoke();
    }

    public int GetCurrentKills() => currentKills;

    public void ResetScore()
    {
        currentKills = 0;
        bossSpawned = false;
        victoryTriggered = false;
        OnScoreChanged?.Invoke(currentKills);
    }
}