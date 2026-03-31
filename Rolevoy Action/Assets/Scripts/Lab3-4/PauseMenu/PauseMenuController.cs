using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private PauseMenuView view;
    private ISaveService saveService;
    private Player player;
    private PlayerController playerController;

    public PauseMenuController(PauseMenuView view, ISaveService saveService, Player player, PlayerController playerController)
    {
        this.view = view;
        this.saveService = saveService;
        this.player = player;
        this.playerController = playerController;

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
        data.PlayerPosition = playerController.transform.position;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        data.EnemyPositions = new System.Collections.Generic.List<Vector3>();
        data.EnemyHealths = new System.Collections.Generic.List<float>();


        foreach (var enemy in enemies)
        {
            data.EnemyPositions.Add(enemy.transform.position);
            var ihealth = enemy.GetComponent<IHealth>();
            data.EnemyHealths.Add(ihealth != null ? ihealth.Current : 100f);

        }

        saveService.Save(data);
        Debug.Log("Game saved");
    }

    private void Load()
    {
        GameData data = saveService.Load();
        if (data != null)
        {
            player.Health.Restore(data.PlayerHP);
            playerController.Teleport(data.PlayerPosition);

            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (data.EnemyPositions != null && data.EnemyPositions.Count == enemies.Length)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].transform.position = data.EnemyPositions[i];
                    var ihealth = enemies[i].GetComponent<IHealth>();
                    if (ihealth != null && i < data.EnemyHealths.Count)
                        ihealth.Restore(data.EnemyHealths[i]);

                }
            }
            Debug.Log("Game loaded");
        }
    }
}