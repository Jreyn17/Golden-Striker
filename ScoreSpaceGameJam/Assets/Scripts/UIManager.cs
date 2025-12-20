using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text pointsText;
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
        GameManager.OnGameStarted += HandleGameStarted;
        GameManager.OnGameOver += HandleGameOver;
    }
    
    //Unsubscribe to Events
    void OnDisable()
    {
        GameManager.OnLivesChanged -= HandleLivesChanged;
        GameManager.OnPointsChanged -= HandlePointsChanged;
        GameManager.OnGameStarted -= HandleGameStarted;
        GameManager.OnGameOver -= HandleGameOver;
    }

    //When lives increase or decrease
    private void HandleLivesChanged(int lives)
    {
        if (livesText != null) livesText.text = $"Lives: {lives}";
    }

    //When points increase
    private void HandlePointsChanged(int points)
    {
        if (pointsText != null) pointsText.text = $"Points: {points}";
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
