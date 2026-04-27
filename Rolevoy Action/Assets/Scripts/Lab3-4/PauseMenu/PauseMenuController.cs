using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private PauseMenuView view;
    private GameInteractor interactor;
    private Player player;
    private PlayerController playerController;
    private IPeacefulModeService peacefulService;

    public PauseMenuController(PauseMenuView view, GameInteractor interactor, Player player, PlayerController playerController)
    {
        this.view = view;
        this.interactor = interactor;
        this.player = player;
        this.playerController = playerController;

        view.resumeButton.onClick.AddListener(Resume);
        view.mainMenuButton.onClick.AddListener(Exit);
        view.saveButton.onClick.AddListener(Save);
        view.loadButton.onClick.AddListener(Load);
        view.peacefulModeToggle.isOn = peacefulService.IsPeaceful;
        view.peacefulModeToggle.onValueChanged.AddListener(OnPeacefulToggled);
    }

    private void OnPeacefulToggled(bool isOn)
    {
        peacefulService.IsPeaceful = isOn;
        Debug.Log("Peaceful mode: " + (isOn ? "ON" : "OFF"));
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
        interactor.SaveGame(player, playerController);
    }

    private void Load()
    {
        interactor.LoadGame(player, playerController);
    }
}