using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        panel.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void Show()
    {
        Debug.Log("GameOverUI.Show() called");
        if (panel == null) Debug.LogError("panel is null");
        else
        {
            panel.SetActive(true);
            Debug.Log("panel active = " + panel.activeSelf);
        }
        Time.timeScale = 0;
    }

    private void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowVictory()
    {
        // Меняем текст, если есть
        var text = panel.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = "VICTORY!";

        panel.SetActive(true);
        Time.timeScale = 0;
    }
}