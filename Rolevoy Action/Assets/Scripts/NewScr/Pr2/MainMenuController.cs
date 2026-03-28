using UnityEngine.SceneManagement;

public class MainMenuController
{
    private MainMenuView view;

    public MainMenuController(MainMenuView view)
    {
        this.view = view;

        view.playButton.onClick.AddListener(OnPlay);
        view.settingsButton.onClick.AddListener(OnSettings);
    }

    private void OnPlay()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnSettings()
    {
        SceneManager.LoadScene("Settings");
    }
}