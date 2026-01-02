using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Events
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnPointsChanged;
    public static event Action<int> OnHighScore;
    public static event Action OnGameStarted;
    public static event Action OnGameOver;
    public static event Action<int> OnLevelUp;
    public static event Action<string, int> OnSubmitScore;

    [SerializeField] private ProfileManager profileManager;

    //Panels
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private GameObject settingsPanel;

    private int startingLives = 3;

    public int points { get; private set; }
    public int highScore { get; private set; }
    public int lives { get; private set; }
    public bool IsRunning { get; private set; } //Is game running?
    public int startingHighScore { get; private set; } //Store this for leaderboard

    private int level = 1; //Levels
    private int nextLevelUpScore = 5; //IF I WANT IT TO BE EVERY CERTAIN AMOUNT OF POINTS. ALSO COULD MAKE IT A LIST FOR SPECIFIC VALUES

    [SerializeField] AudioSource buttonSource;
    [SerializeField] AudioClip buttonClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        ResetGameState();
    }

    private void ResetGameState()
    {
        points = 0;
        highScore = PlayerPrefs.GetInt("hs101", 0);
        startingHighScore = highScore;
        lives = Mathf.Max(0, startingLives);
        IsRunning = false;

        level = 1;
        nextLevelUpScore = 5;

        OnPointsChanged?.Invoke(points);
        OnLivesChanged?.Invoke(lives);
    }

    #region Buttons
    public void OnPlayClicked()
    {

        ResetGameState();
        IsRunning = true;

        OnGameStarted?.Invoke();
        OnHighScore?.Invoke(highScore);

        buttonSource.PlayOneShot(buttonClick);

        Cursor.visible = false;
    }

    public void OnLeaderboardClicked()
    {
        if (leaderboardPanel.activeSelf)
        {
            leaderboardPanel.SetActive(false);
        }
        else
        {
            leaderboardPanel.SetActive(true);
        }

        buttonSource.PlayOneShot(buttonClick);
    }

    public void OnRulesClicked()
    {
        if (rulesPanel.activeSelf)
        {
            rulesPanel.SetActive(false);
        }
        else
        {
            rulesPanel.SetActive(true);
        }

        buttonSource.PlayOneShot(buttonClick);
    }

    public void OnSettingsClicked()
    {
        if (settingsPanel.activeSelf) settingsPanel.SetActive(false);
        else settingsPanel.SetActive(true);

        buttonSource.PlayOneShot(buttonClick);
    }
    #endregion

    public void UpdatePoints(int amount)
    {
        if (!IsRunning) return;
        if (lives <= 0) return;

        points += amount; //Add points
        OnPointsChanged?.Invoke(points); //Invoke subs (UIManager)

        if (points > highScore)
        {
            Debug.Log(points);
            highScore = points;
            PlayerPrefs.SetInt("hs101", highScore);
            PlayerPrefs.Save();
            OnHighScore?.Invoke(highScore);
        }

        while (points >= nextLevelUpScore)
        {
            level++;
            OnLevelUp?.Invoke(level);

            nextLevelUpScore += 5;
        }
    }

    //Might need to change to ChangeLife (if shop added)
    public void LoseLife(int amount)
    {
        if (!IsRunning) return;
        if (lives <= 0) return;

        //Subtract lives
        lives = Mathf.Max(0, lives - amount);
        OnLivesChanged?.Invoke(lives); //Invoke subs (UIManager)

        //If no more lives, initiate GameOver sequence
        if (lives == 0)
        {
            if (highScore > startingHighScore)
            {
                SubmitScore();
            }

            IsRunning = false;

            Cursor.visible = true;

            OnGameOver?.Invoke();
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            
            foreach(GameObject ball in balls) 
            {
                Destroy(ball);
            }
        }
    }

    private void SubmitScore()
    {
        OnSubmitScore?.Invoke(profileManager.usernameInput.text, highScore);
    }
}
