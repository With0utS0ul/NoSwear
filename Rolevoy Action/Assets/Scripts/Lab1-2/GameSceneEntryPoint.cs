using System.Collections;
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
        var saveService = GameEntryPoint.Instance != null
    ? GameEntryPoint.Instance.SaveService
    : new PlayerPrefsSaveService();

        var health = new Health(100);
        player = new Player(health, damageService);

        player.OnDeath += () => gameOverUI.Show();

        playerView.Init(player);
        playerController.Init(player);

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
            new PauseMenuController(pauseView, saveService, player, playerView.transform);

        player.OnDeath += () => StartCoroutine(ShowGameOverDelayed());
    }
    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameOverUI.Show();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}