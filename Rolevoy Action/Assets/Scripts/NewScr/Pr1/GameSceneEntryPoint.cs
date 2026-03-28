using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EntityView playerView;
    [SerializeField] private GameOverUI gameOverUI;

    private Player player;

    [System.Obsolete]
    private void Awake()
    {
        var damageService = new DamageService();
        var saveService = GameEntryPoint.Instance.SaveService;

        var health = new Health(100);
        player = new Player(health, damageService);

        player.OnDeath += () => gameOverUI.Show();

        playerView.Init(player);
        playerController.Init(player);

        // Инициализация HealthBar для игрока
        var playerHealthBar = playerView.GetComponentInChildren<HealthBar>();
        if (playerHealthBar != null)
            playerHealthBar.Init(health);

        // Инициализация HealthBar для врагов
        foreach (var enemy in FindObjectsOfType<EnemyMeleeAI>())
        {
            var healthComp = enemy.GetComponent<HealthComp>();
            var bar = enemy.GetComponentInChildren<HealthBar>();
            if (healthComp != null && bar != null)
                bar.Init(healthComp);
        }
        foreach (var enemy in FindObjectsOfType<EnemyMagAI>())
        {
            var healthComp = enemy.GetComponent<HealthComp>();
            var bar = enemy.GetComponentInChildren<HealthBar>();
            if (healthComp != null && bar != null)
                bar.Init(healthComp);
        }

        // Инициализация паузы
        var pauseView = FindObjectOfType<PauseMenuView>(true);
        if (pauseView != null)
            new PauseMenuController(pauseView, saveService, player, playerView.transform);
    }
}