using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private int killsToSpawnBoss = 3;
    [SerializeField] private int killsToWin = 5;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [Header("Events")]
    public UnityEvent<int> OnScoreChanged; // текущее количество убийств
    public UnityEvent OnBossSpawned;
    public UnityEvent OnVictory;

    private int currentKills = 0;
    private bool bossSpawned = false;
    private bool victoryTriggered = false;
    private HashSet<EnemyView> reportedDeaths = new HashSet<EnemyView>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterEnemyDeath(EnemyView enemy, bool isBoss = false)
    {
        if (reportedDeaths.Contains(enemy)) return;
        reportedDeaths.Add(enemy);

        if (victoryTriggered) return;

        if (!isBoss)
        {
            currentKills++;
            OnScoreChanged?.Invoke(currentKills);

            if (!bossSpawned && currentKills >= killsToSpawnBoss)
            {
                SpawnBoss();
            }

            if (!victoryTriggered && currentKills >= killsToWin)
            {
                TriggerVictory();
            }
        }
        else
        {
            // Если босс убит – можно сразу победу (опционально)
            if (!victoryTriggered)
                TriggerVictory();
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossSpawned = true;
            OnBossSpawned?.Invoke();
            Debug.Log("Boss spawned!");
        }
        else
        {
            Debug.LogError("Boss prefab or spawn point not set in ScoreManager!");
        }
    }

    private void TriggerVictory()
    {
        if (victoryTriggered) return;
        victoryTriggered = true;
        OnVictory?.Invoke();
        

        // Проигрываем победную мелодию через AudioService
        var audio = GameEntryPoint.Instance?.AudioService;
        if (audio != null)
        {
            // Предположим, у вас есть AudioClip victoryMusic, загрузите его
            AudioClip victoryClip = Resources.Load<AudioClip>("Music/Victory");
            if (victoryClip != null)
                audio.PlayMusic(victoryClip);
            else
                Debug.LogWarning("Victory music clip not found!");
        }


     
       
    }

   

    public int GetCurrentKills() => currentKills;
}