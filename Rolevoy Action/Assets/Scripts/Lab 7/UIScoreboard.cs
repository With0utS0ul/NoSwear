using UnityEngine;
using UnityEngine.UI;

public class UIScoreboard : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private ScoreManager _scoreManager;

    
    public void Init(ScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
        _scoreManager.OnScoreChanged += UpdateScore;
        UpdateScore(_scoreManager.GetCurrentKills());
    }

    private void UpdateScore(int kills)
    {
        if (scoreText != null)
            scoreText.text = $"Kills: {kills}";
    }

    private void OnDestroy()
    {
        if (_scoreManager != null)
            _scoreManager.OnScoreChanged -= UpdateScore;
    }
}