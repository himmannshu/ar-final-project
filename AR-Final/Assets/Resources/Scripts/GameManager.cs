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
    public TextMeshProUGUI ScoreText;     
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI HealthText; 
    public GameObject GameOverPanel; 
    //public UnityEvent OnGameOver;

    private int score = 0;
    private float timer;
    private bool isGameOver = false;

    // public int Score => score;
    // public float Timer => timer;
    // public bool IsGameOver => isGameOver;

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
        PlayerHealth.OnHealthChanged.AddListener(UpdateHealthUI); // Subscribe to update UI

        UpdateScoreUI();
        UpdateTimerUI();
        UpdateHealthUI(PlayerHealth.CurrentHealth, PlayerHealth.MaxHealth); // Initial health UI update
    }

    void Update()
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();

            if (timer <= 0f)
            {
                timer = 0f; // Clamp timer display
                UpdateTimerUI(); // Update UI one last time for 00:00
                EndGame("Time's Up!");
            }
        }
    }

    void OnDestroy()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.OnPlayerDeath.RemoveListener(HandlePlayerDeath);
            PlayerHealth.OnHealthChanged.RemoveListener(UpdateHealthUI);
        }

        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return; 

        score += points;
        UpdateScoreUI();
        Debug.Log($"Score increased by {points}. Total Score: {score}");
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

        
        //OnGameOver?.Invoke();

        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f; 
    }


    private void UpdateScoreUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"Score: {score}";
        }
    }

    private void UpdateTimerUI()
    {
        if (TimerText != null)
        {
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            TimerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    private void UpdateHealthUI(float current, float max)
    {
        if (HealthText != null)
        {
            HealthText.text = $"Health: {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}"; 
        }
    }
}
