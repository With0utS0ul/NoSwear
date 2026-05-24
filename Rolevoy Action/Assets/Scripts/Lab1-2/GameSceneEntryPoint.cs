using System.Collections;
using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EntityView playerView;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private ScoreManager scoreManager;

    private Player player;

    [System.Obsolete]
    private void Awake()
    {
        var damageService = new DamageService();
        var saveService = GameEntryPoint.Instance != null
    ? GameEntryPoint.Instance.SaveService
    : new PlayerPrefsSaveService();
        

        IPlayerRepository playerRepo = new PlayerRepository();
        IEnemiesRepository enemiesRepo = new EnemiesRepository();
        GameInteractor interactor = new GameInteractor(playerRepo, enemiesRepo, saveService);

        var health = new Health(100);
        player = new Player(health, damageService);

        player.OnDeath += () => gameOverUI.Show();
        scoreManager.OnVictory.AddListener(ShowVictoryUI);

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
                interactor,
                player,
                playerController,
                GameEntryPoint.Instance?.PeacefulModeService);

        player.OnDeath += () => StartCoroutine(ShowGameOverDelayed());
    }
    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameOverUI.Show();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
}