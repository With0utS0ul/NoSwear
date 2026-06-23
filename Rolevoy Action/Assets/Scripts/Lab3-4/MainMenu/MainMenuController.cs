using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController
{
    private readonly MainMenuView _view;

    public MainMenuController(MainMenuView view)
    {
        _view = view;

        // Подписываемся на абстрактные события логики, а не на элементы UI
        _view.OnPlayClicked += HandlePlay;
        _view.OnSettingsClicked += HandleSettings;
        _view.OnExitClicked += HandleExit;
    }

    // Метод для очистки подписок при уничтожении сцены
    public void Dispose()
    {
        _view.OnPlayClicked -= HandlePlay;
        _view.OnSettingsClicked -= HandleSettings;
        _view.OnExitClicked -= HandleExit;
    }

    private void HandlePlay()
    {
        Dispose(); // Чистим за собой перед сменой сцены
        SceneManager.LoadScene("Game");
    }

    private void HandleSettings()
    {
        Dispose();
        SceneManager.LoadScene("Settings");
    }

    private void HandleExit()
    {
        Application.Quit();
    }
}