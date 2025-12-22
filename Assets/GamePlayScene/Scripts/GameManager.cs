using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI ScoreText;   
    public TextMeshProUGUI ArrowsText; 

    [Header("Game Settings")]
    public int scorePerHit = 200;

    private int score = 0;
    private int arrows = 0;
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void ArrowFired()
    {
        if (gameOver) return;

        arrows++;
        UpdateUI();
    }

    public void ArrowHit()
    {
        if (gameOver) return;

        score += scorePerHit;
        UpdateUI();
    }

    public void ArrowMissed()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Game Over!");

        MenuCanvasController menu = FindObjectOfType<MenuCanvasController>();
        if (menu != null)
            menu.ShowGameOver();
        GameOverPanel gameOverPanel = FindObjectOfType<GameOverPanel>();
        if (gameOverPanel != null)
            gameOverPanel.ShowGameOver(score);
    }

    private void UpdateUI()
    {
        if (ScoreText != null)
            ScoreText.text = $"Score: {score}";
        if (ArrowsText != null)
            ArrowsText.text = $"Arrows: {arrows}";
    }
}
