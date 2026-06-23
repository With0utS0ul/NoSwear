using System.Collections;
using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private int killsToWin;
    [SerializeField] private int killsToSpawnBoss;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EntityView playerView;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private AudioClip gameBackgroundMusic;



    private Player player;

    [System.Obsolete]
    private void Awake()
    {

        var audioService = GameEntryPoint.Instance?.AudioService;
        if (audioService != null && gameBackgroundMusic != null)
        {
            audioService.PlayMusic(gameBackgroundMusic);
        }

        var damageService = new DamageService();
        var saveService = GameEntryPoint.Instance != null
            ? GameEntryPoint.Instance.SaveService
            : new PlayerPrefsSaveService();


        IPlayerRepository playerRepo = new PlayerRepository();
        IEnemiesRepository enemiesRepo = new EnemiesRepository();
        SaveGameInteractor saveinteractor = new SaveGameInteractor(playerRepo, enemiesRepo, saveService);
        LoadGameInteractor loadinteractor = new LoadGameInteractor(playerRepo, enemiesRepo, saveService);

        var health = new Health(100);
        player = new Player(health, damageService);




        playerView.Init(player);
        playerController.Init(player);
        var cooldownUI = FindObjectOfType<CooldownUI>();
        if (cooldownUI != null)
            cooldownUI.Init(player);

        var playerHealthBar = playerView.GetComponentInChildren<HealthBar>();
        if (playerHealthBar != null)
            playerHealthBar.Init(health);

        var scoreManager = new ScoreManager(killsToSpawnBoss, killsToWin, bossPrefab, bossSpawnPoint);

        // Инициализируем UI счета
        var scoreboard = FindObjectOfType<UIScoreboard>();
        if (scoreboard != null)
        {
            scoreboard.Init(scoreManager);
        }

        // Инициализируем UI окончания игры
        if (gameOverUI != null)
        {
            gameOverUI.Init(player, scoreManager);
        }


        var spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
        {
            spawner.OnEnemySpawned += (newEnemy) =>
            {
                InitEnemy(newEnemy, scoreManager);
            };
        }

        var pauseInput = FindObjectOfType<PauseMenuInput>(true);
        var pauseView = FindObjectOfType<PauseMenuView>(true);
        if (pauseView != null)
        {
            PauseMenuController pauseController = new PauseMenuController(
                pauseView,
                saveinteractor,
                loadinteractor,
                player,
                playerController,
                GameEntryPoint.Instance?.PeacefulModeService);

            if (pauseInput != null)
                pauseInput.Initialize(pauseController);

        }


    }

    private void InitEnemy(EnemyView enemy, ScoreManager scoreManager)
    {
        if (enemy == null) return;

        // Инициализируем полоску здоровья над головой врага
        var bar = enemy.GetComponentInChildren<HealthBar>();
        if (bar != null && enemy.Enemy != null)
        {
            bar.Init(enemy.Enemy.Health);
        }

        // Подписываемся на событие смерти этого конкретного врага
        enemy.OnDied += (deadEnemy) =>
        {
            bool isBoss = deadEnemy.GetComponent<BossTag>() != null;
            scoreManager.RegisterEnemyDeath(deadEnemy, isBoss);
        };
    }


}