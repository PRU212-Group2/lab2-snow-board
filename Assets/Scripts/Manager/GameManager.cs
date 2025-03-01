using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    [SerializeField] int progressScorePerSecond = 10;
    [SerializeField] int trickBaseScore = 500;
    [SerializeField] float comboTimeWindow = 5f;
    [SerializeField] int maxSkillChained = 5;
    
    private float lastScoreUpdateTime;
    private float lastTrickTime;
    private int comboCounter = 0;
    private int score;
    
    static GameManager _instance;
    
    void Awake()
    {
        ManageSingleton();
    }

    void Start()
    {
        lastScoreUpdateTime = Time.time;
    }
    
    void Update()
    {
        // Add progressive score over time
        if (Time.time - lastScoreUpdateTime >= 1f)
        {
            AddScore(progressScorePerSecond);
            lastScoreUpdateTime = Time.time;
        }
        
        // Check if combo has expired
        if (comboCounter > 0 && Time.time - lastTrickTime > comboTimeWindow)
        {
            comboCounter = 0;
        }
    }

    // Applying singleton pattern
    void ManageSingleton()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // Public method to retrieve score
    public int GetScore()
    {
        return score;
    }

    // Add player score based on events
    public void AddScore(int amount)
    {
        score += amount;
        
        // Clamp the score to not be less than 0
        score = Mathf.Clamp(score, 0, int.MaxValue);
    }
    
    // Add score with a text indicator
    public void AddScoreWithIndicator(int amount, string indicatorText, Vector3 position)
    {
        AddScore(amount);
        UIDisplay display = FindFirstObjectByType<UIDisplay>();
        if (display != null)
        {
            display.ShowScoreIndicator(amount, indicatorText, position);
        }
    }
    
    // Handle trick completion
    public void CompleteTrick()
    {
        comboCounter++;
        int multiplier = Mathf.Min(comboCounter, maxSkillChained); // Cap at 5x multiplier
        int trickScore = trickBaseScore * multiplier;
        
        string indicatorText = comboCounter > 1 ? 
            $"360° Trick x{multiplier}" : 
            "360° Trick";
            
        // Get player position for the indicator
        PlayerController player = FindFirstObjectByType<PlayerController>();
        Vector3 position = player != null ? player.transform.position : Vector3.zero;
        
        AddScoreWithIndicator(trickScore, indicatorText, position);
        
        lastTrickTime = Time.time;
    }

    // Reset player score
    public void ResetScore()
    {
        score = 0;
        comboCounter = 0;
    }
    
    // Either reset the current level or the whole game session
    public void ProcessPlayerCrash()
    {
        Invoke("ResetGame", loadDelay);
    }

    // Reset game session to the first level
    void ResetGame()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}