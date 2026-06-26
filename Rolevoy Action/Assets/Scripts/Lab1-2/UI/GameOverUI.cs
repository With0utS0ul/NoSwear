using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private Player player;
    private ScoreManager scoreManager;

    private void Start()
    {
        panel.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

   
    public void Init(Player player, ScoreManager scoreManager)
    {
        this.player = player;
        this.scoreManager = scoreManager;

        if (this.player != null)
            this.player.Health.OnDeath += ShowGameOver;

        if (this.scoreManager != null)
            this.scoreManager.OnVictory += ShowVictory;
    }
    private void OnDestroy()
    {
        if (player != null)
            player.Health.OnDeath -= ShowGameOver;

        if (scoreManager != null)
            scoreManager.OnVictory -= ShowVictory;
    }

    public void ShowGameOver()
    {
        SetPanelActive(true);
    }

    private void Restart()
    {
        SetPanelActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        SetPanelActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowVictory()
    {
        var text = panel.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = "VICTORY!";

        SetPanelActive(true);
    }

    private void SetPanelActive(bool isActive)
    {
        if (panel != null)
            panel.SetActive(isActive);

        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}