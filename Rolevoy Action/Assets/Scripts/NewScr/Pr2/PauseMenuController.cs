using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private PauseMenuView view;
    private ISaveService saveService;
    private Player player;
    private Transform playerTransform;

    public PauseMenuController(PauseMenuView view, ISaveService saveService, Player player, Transform playerTransform)
    {
        this.view = view;
        this.saveService = saveService;
        this.player = player;
        this.playerTransform = playerTransform;

        view.resumeButton.onClick.AddListener(Resume);
        view.mainMenuButton.onClick.AddListener(Exit);
        view.saveButton.onClick.AddListener(Save);
        view.loadButton.onClick.AddListener(Load);
    }

    private void Resume()
    {
        view.root.SetActive(false);
        Time.timeScale = 1;
    }

    private void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void Save()
    {
        GameData data = new GameData();
        data.PlayerHP = player.Health.Current;
        data.PlayerPosition = playerTransform.position;

        // ƒоп. балл: сохранить позиции всех врагов с тегом "Enemy"
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        data.EnemyPositions = new System.Collections.Generic.List<Vector3>();
        foreach (var enemy in enemies)
            data.EnemyPositions.Add(enemy.transform.position);

        saveService.Save(data);
        Debug.Log("Game saved");
    }

    private void Load()
    {
        GameData data = saveService.Load();
        if (data != null)
        {
            player.Health.Restore(data.PlayerHP);
            playerTransform.position = data.PlayerPosition;

            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (data.EnemyPositions != null && data.EnemyPositions.Count == enemies.Length)
            {
                for (int i = 0; i < enemies.Length; i++)
                    enemies[i].transform.position = data.EnemyPositions[i];
            }
            Debug.Log("Game loaded");
        }
    }
}