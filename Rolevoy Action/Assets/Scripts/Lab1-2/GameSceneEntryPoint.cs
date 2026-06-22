using System.Collections;
using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EntityView playerView;
    [SerializeField] private GameOverUI gameOverUI;
    

    private Player player;

    [System.Obsolete]
    private void Awake()
    {
        var audioService = GameEntryPoint.Instance?.AudioService;
        if (audioService != null)
        {
            AudioClip gameMusic = Resources.Load<AudioClip>("Music/GameBackground"); // Укажи свой путь в Resources
            if (gameMusic != null)
                audioService.PlayMusic(gameMusic);
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

        foreach (var enemy in FindObjectsOfType<EnemyView>())
        {
            var bar = enemy.GetComponentInChildren<HealthBar>();

            if (bar != null && enemy.Enemy != null)
                bar.Init(enemy.Enemy.Health);
        }

        var pauseView = FindObjectOfType<PauseMenuView>(true);
        if (pauseView != null)
            new PauseMenuController(
                pauseView,
                saveinteractor,
                loadinteractor,
                player,
                playerController,
                GameEntryPoint.Instance?.PeacefulModeService);


        player.OnDeath += () => StartCoroutine(ShowGameOverDelayed());

        if (Bootstrapper.Instance?.ScoreManager != null)
        {
            Bootstrapper.Instance.ScoreManager.ResetScore();

            Bootstrapper.Instance.ScoreManager.UpdateSpawnPoint(bossSpawnPoint);

            Bootstrapper.Instance.ScoreManager.OnVictory += ShowVictoryUI;
        }
        else
        {
            Debug.LogWarning("Bootstrapper или ScoreManager не найдены! Запусти игру со стартовой сцены.");
        }

        
    }
    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (gameOverUI != null)
        {
            gameOverUI.Show();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ShowVictoryUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (gameOverUI != null)
            gameOverUI.ShowVictory();

        
        else
            Debug.LogError("GameOverUI not assigned in GameSceneEntryPoint");
    }
    private void OnDestroy()
    {
        // Отписываемся ОБОИМИ способами, чтобы точно убрать старую ссылку из вечного ScoreManager
        if (Bootstrapper.Instance != null && Bootstrapper.Instance.ScoreManager != null)
        {
            Bootstrapper.Instance.ScoreManager.OnVictory -= ShowVictoryUI;
        }
    }
}