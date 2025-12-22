using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI ScoreText;      
    public TextMeshProUGUI BestScoreText;  

    public void ShowGameOver(int score)
    {
        if (score > PlayerData.Instance.BestScore)
        {
            PlayerData.Instance.BestScore = score;
        }

        if (ScoreText != null)
            ScoreText.text = $"Score: {score}";

        if (BestScoreText != null)
            BestScoreText.text = $"Best Score: {PlayerData.Instance.BestScore}";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
