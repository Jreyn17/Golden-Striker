using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject ball;

    [Header("Time")]
    private float spawnInterval = 1f; //Starting interval is 5       //THIS WILL SCALE WITH LEVELS
    private float timer = 0f; //Timer
    private bool timerRunning = false; //If game is running, timer is running

    void OnEnable()
    {
        GameManager.OnGameStarted += StartTimer;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= StartTimer;
    }

    void Update()
    {
        if (!timerRunning)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBall();
            timer = 0f;
        }
    }

    private void StartTimer()
    {
        timerRunning = true;
    }

    private void SpawnBall()
    {
        Instantiate(ball, new Vector2(Random.Range(-4f, 4f), Random.Range(-10f, -6f)), Quaternion.Euler(0f, 0f, Random.Range(0f, 180f)));
    }
}
