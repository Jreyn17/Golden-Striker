using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text streakText;
    [SerializeField] private TMP_Text highScoreText;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject statsPanel;

    void Start()
    {
        statsPanel.SetActive(false);
    }

    //Subscribe to Events
    void OnEnable()
    {
        GameManager.OnLivesChanged += HandleLivesChanged;
        GameManager.OnPointsChanged += HandlePointsChanged;
        GameManager.OnStreakChanged += HandleStreakChanged;
        GameManager.OnHighScore += HandleHighScore;
        GameManager.OnGameStarted += HandleGameStarted;
        GameManager.OnGameOver += HandleGameOver;
    }
    
    //Unsubscribe to Events
    void OnDisable()
    {
        GameManager.OnLivesChanged -= HandleLivesChanged;
        GameManager.OnPointsChanged -= HandlePointsChanged;
        GameManager.OnStreakChanged -= HandleStreakChanged;
        GameManager.OnHighScore -= HandleHighScore;
        GameManager.OnGameStarted -= HandleGameStarted;
        GameManager.OnGameOver -= HandleGameOver;
    }

    //When lives increase or decrease
    private void HandleLivesChanged(int lives)
    {
        if (livesText != null) livesText.text = $"{lives}";
    }

    //When points increase
    private void HandlePointsChanged(int points)
    {
        if (pointsText != null) pointsText.text = $"{points}";
        
    }

    //When streak changes
    private void HandleStreakChanged(int streak, int nextStreakUp)
    {
        if (streakText != null) streakText.text = $"{streak}/{nextStreakUp}";
    }

    private void HandleHighScore(int highScore)
    {
        if (highScoreText != null) highScoreText.text = $"{highScore}";
    }

    //When game starts
    private void HandleGameStarted()
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (statsPanel != null) statsPanel.SetActive(true);
    }

    //When game ends
    private void HandleGameOver()
    {
        if (startPanel != null) startPanel.SetActive(true);
        if (statsPanel != null) statsPanel.SetActive(false);
    }
}
