using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuController
{
    private PauseMenuView view;
    private ISaveService saveService;
    private Player player;

    public PauseMenuController(PauseMenuView view, ISaveService saveService, Player player)
    {
        this.view = view;
        this.saveService = saveService;
        this.player = player;

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
        SceneManager.LoadScene("MainMenu");
    }

    private void Save()
    {
        saveService.Save(new GameData
        {
            PlayerHP = playerHealth(),
            PlayerPosition = playerPosition()
        });
    }

    private void Load()
    {
        var data = saveService.Load();
        // применим позже (Лаба 4)
    }

    private float playerHealth() => 100; // временно
    private Vector3 playerPosition() => Vector3.zero;
}