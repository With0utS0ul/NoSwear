using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    public static Bootstrapper Instance { get; private set; }
    public ScoreManager ScoreManager { get; private set; }

    [Header("ScoreManager Settings")]
    [SerializeField] private int killsToSpawnBoss = 3;
    [SerializeField] private int killsToWin = 5;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ScoreManager = new ScoreManager(killsToSpawnBoss, killsToWin, bossPrefab, bossSpawnPoint);
    }
}