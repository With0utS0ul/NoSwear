using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly SaveGameInteractor _saveInteractor;
    private readonly LoadGameInteractor _loadInteractor;
    private readonly Player _player;
    private readonly PlayerController _playerController;
    private readonly IPeacefulModeService _peacefulService;

    public PauseMenuController(
        PauseMenuView view,
        SaveGameInteractor saveInteractor,
        LoadGameInteractor loadInteractor,
        Player player,
        PlayerController playerController,
        IPeacefulModeService peacefulService)
    {
        _view = view;
        _saveInteractor = saveInteractor;
        _loadInteractor = loadInteractor;
        _player = player;
        _playerController = playerController;
        _peacefulService = peacefulService;

        // Инициализируем начальное состояние тогла
        if (_peacefulService != null)
        {
            _view.SetPeacefulToggleState(_peacefulService.IsPeaceful);
        }

        // Подписываемся на абстрактные события View
        _view.OnResumeClicked += Resume;
        _view.OnMainMenuClicked += ExitToMainMenu;
        _view.OnSaveClicked += Save;
        _view.OnLoadClicked += Load;
        _view.OnPeacefulToggled += HandlePeacefulToggled;
    }

    // Этот метод теперь будет вызываться при нажатии Esc
    public void TogglePause()
    {
        bool targetState = !_view.IsActive;
        _view.SetActive(targetState);
        Time.timeScale = targetState ? 0f : 1f;
    }

    private void HandlePeacefulToggled(bool isOn)
    {
        if (_peacefulService != null)
        {
            _peacefulService.IsPeaceful = isOn;
            Debug.Log("Peaceful mode: " + (isOn ? "ON" : "OFF"));
        }
    }

    private void Resume()
    {
        _view.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        Dispose(); // Обязательно чистим подписки перед сменой сцены
        SceneManager.LoadScene("MainMenu");
    }

    private void Save()
    {
        _saveInteractor.SaveGame(_player, _playerController);
    }

    private void Load()
    {
        _loadInteractor.LoadGame(_player, _playerController);
    }

    public void Dispose()
    {
        _view.OnResumeClicked -= Resume;
        _view.OnMainMenuClicked -= ExitToMainMenu;
        _view.OnSaveClicked -= Save;
        _view.OnLoadClicked -= Load;
        _view.OnPeacefulToggled -= HandlePeacefulToggled;
    }
}