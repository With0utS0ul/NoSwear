using UnityEngine;
using UnityEngine.UI;

public class UIScoreboard : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void Start()
    {
        var scoreManager = Bootstrapper.Instance?.ScoreManager;
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged += UpdateScore;
            UpdateScore(scoreManager.GetCurrentKills());
        }
    }

    private void UpdateScore(int kills)
    {
        if (scoreText != null)
            scoreText.text = $"Kills: {kills}";
    }

    private void OnDestroy()
    {
        var scoreManager = Bootstrapper.Instance?.ScoreManager;
        if (scoreManager != null)
            scoreManager.OnScoreChanged -= UpdateScore;
    }
}