using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.Events;        
using TMPro; 

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }

    public float GameDurationSeconds = 60f; 
    public PlayerHealth PlayerHealth; 
    // public TextMeshProUGUI ScoreText;     
    // public TextMeshProUGUI TimerText;
    // public TextMeshProUGUI HealthText; 
    public GameObject GameOverPanel; 
    public UnityEvent OnScoreUpdated;

    private int score = 0;
    private float timer;
    private bool isGameOver = false;

    public int Score => score;
    public float Timer => timer;
    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
        else
        {
            _instance = this;
        }

        timer = GameDurationSeconds;
        if (GameOverPanel != null) GameOverPanel.SetActive(false); 
    }

    void Start()
    {

        PlayerHealth.OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    void Update()
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
                EndGame("Time's Up!");
            }
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return; 

        score += points;

        Debug.Log($"Score increased by {points}. Total Score: {score}");
        OnScoreUpdated?.Invoke(); 
    }

    public void RestartGame()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void HandlePlayerDeath()
    {
        EndGame("Player Defeated!");
    }

    private void EndGame(string reason)
    {
        if (isGameOver) return; 

        isGameOver = true;
        timer = 0f; 
        Debug.Log($"Game Over! Reason: {reason}. Final Score: {score}");

        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f; 
    }

}
