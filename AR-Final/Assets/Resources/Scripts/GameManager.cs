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

    public float gameDurationSeconds = 60f; 
    public PlayerHealth playerHealth; 
    public TextMeshProUGUI scoreText;     
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI healthText; 
    public GameObject gameOverPanel; 
    public UnityEvent OnGameOver;

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

        timer = gameDurationSeconds;
        if (gameOverPanel != null) gameOverPanel.SetActive(false); // Hide game over screen initially
    }

    void Start()
    {

        playerHealth.OnPlayerDeath.AddListener(HandlePlayerDeath);
        playerHealth.OnHealthChanged.AddListener(UpdateHealthUI); // Subscribe to update UI

        UpdateScoreUI();
        UpdateTimerUI();
        UpdateHealthUI(playerHealth.CurrentHealth, playerHealth.MaxHealth); // Initial health UI update
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
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath.RemoveListener(HandlePlayerDeath);
            playerHealth.OnHealthChanged.RemoveListener(UpdateHealthUI);
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
        if (isGameOver) return; // Prevent ending the game multiple times

        isGameOver = true;
        timer = 0f; // Ensure timer stops at 0
        Debug.Log($"Game Over! Reason: {reason}. Final Score: {score}");

        // Trigger game over event
        OnGameOver?.Invoke();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }


    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    private void UpdateHealthUI(float current, float max)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}"; 
        }
    }
}
