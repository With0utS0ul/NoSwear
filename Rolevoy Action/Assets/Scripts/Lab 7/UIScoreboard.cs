using UnityEngine;
using UnityEngine.UI;

public class UIScoreboard : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged.AddListener(UpdateScore);
            UpdateScore(ScoreManager.Instance.GetCurrentKills());
        }
    }

    private void UpdateScore(int kills)
    {
        if (scoreText != null)
            scoreText.text = $"Kills: {kills}";
    }
}