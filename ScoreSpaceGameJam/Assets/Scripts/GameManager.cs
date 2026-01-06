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
    public static event Action<int, int> OnStreakChanged;
    public static event Action<int> OnHighScore;
    public static event Action OnGameStarted;
    public static event Action OnGameOver;
    public static event Action<int> OnLevelUp;
    public static event Action<string, int> OnSubmitScore;

    [SerializeField] private ProfileManager profileManager;
    [SerializeField] private Leaderboard leaderboard;

    //Panels
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private GameObject settingsPanel;

    private int startingLives = 3;

    public int points { get; private set; }
    public int highScore { get; private set; }
    public int streak {  get; private set; }
    public int lives { get; private set; }
    public bool IsRunning { get; private set; } //Is game running?
    public int startingHighScore { get; private set; } //Store this for leaderboard

    private int level = 1; //Levels
    private int nextLevelUpScore; //IF I WANT IT TO BE EVERY CERTAIN AMOUNT OF POINTS. ALSO COULD MAKE IT A LIST FOR SPECIFIC VALUES
    private int nextStreakUpScore; //Streak player must hit to gain an extra life
    private int streakAdder; //Adds to the streakUpScore when player reaches nextStreakUpScore. NextStreakUpScore becomes it when player shoots the ball in the wrong goal or loses a life.

    [SerializeField] private Button currentVersionButton;

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

    private void Start()
    {
        leaderboardPanel.SetActive(false);
        rulesPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void OnEnable()
    {
        NetTrigger.OnPointScored += UpdatePoints;
        NetTrigger.OnWrongGoal += UpdatePoints;
        DamageTrigger.OnLoseLife += LoseLife;
    }

    void OnDisable()
    {
        NetTrigger.OnPointScored -= UpdatePoints;
        NetTrigger.OnWrongGoal -= UpdatePoints;
        DamageTrigger.OnLoseLife -= LoseLife;
    }

    private void ResetGameState()
    {
        points = 0;
        streak = 0;
        highScore = PlayerPrefs.GetInt("hs101", 0);
        startingHighScore = highScore;
        lives = Mathf.Max(0, startingLives);
        IsRunning = false;

        level = 1;
        nextLevelUpScore = 5;
        streakAdder = 10;   
        nextStreakUpScore = streakAdder; 

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

        leaderboard.Leaderboard2Button();
        currentVersionButton.Select();

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

    private void UpdatePoints(int amount)
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

        if (amount > 0) //If a point was gained, add to streak
        {
            streak += 1;
        }
        else //Else, points were lost, therefore streak is lost & amount needed is higher
        {
            streak = 0;
            streakAdder += 1;
            nextStreakUpScore = streakAdder;
        }

        while (streak >= nextStreakUpScore) //Once streak reaches the number, it adds a life and resets the streak
        {
            AddLife(1);

            nextStreakUpScore += streakAdder;
        }

        OnStreakChanged?.Invoke(streak, nextStreakUpScore);

        while (points >= nextLevelUpScore)
        {
            level++;

            nextLevelUpScore += 5;
        }
    }

    //Might need to change to ChangeLife (if shop added)
    private void LoseLife(int amount)
    {
        if (!IsRunning) return;
        if (lives <= 0) return;

        //Subtract lives
        lives = Mathf.Max(0, lives - amount);
        OnLivesChanged?.Invoke(lives); //Invoke subs (UIManager)

        streak = 0; //Streak is lost if a ball goes out of bounds
        streakAdder += 1; //Adds to the streak score needed to get more lives
        nextStreakUpScore = streakAdder;
        OnStreakChanged?.Invoke(streak, nextStreakUpScore);

        //If no more lives, initiate GameOver sequence
        if (lives == 0)
        {
            EndGame();
        }
    }

    private void AddLife(int amount)
    {
        if (!IsRunning) return;
        if (lives <= 0) return;

        //Add lives
        lives = Mathf.Max(0, lives + amount);
        OnLivesChanged?.Invoke(lives); //Invoke subs (UIManager) 
    }

    private void EndGame()
    {
        if (highScore > startingHighScore)
        {
            SubmitScore();
        }

        IsRunning = false;

        Cursor.visible = true;

        OnGameOver?.Invoke();
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }

    private void SubmitScore()
    {
        OnSubmitScore?.Invoke(profileManager.usernameInput.text, highScore);
    }
}
