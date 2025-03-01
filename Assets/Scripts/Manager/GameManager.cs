using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    [SerializeField] int progressScorePerSecond = 10;
    [SerializeField] int trickBaseScore = 300;
    [SerializeField] float comboTimeWindow = 5f;
    [SerializeField] int maxSkillChained = 5;
    
    private float lastScoreUpdateTime;
    private float lastTrickTime;
    private int comboCounter = 0;
    private int score;
    private int highScore;
    
    private string saveFilePath;
    
    static GameManager _instance;
    
    void Awake()
    {
        ManageSingleton();
        // Set the save file path
        saveFilePath = Path.Combine(Application.persistentDataPath, "highscore.json");
        LoadHighScore();
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
    
    // Public method to retrieve high score
    public int GetHighScore()
    {
        return highScore;
    }
    
    // Add player score based on events
    public void AddScore(int amount)
    {
        score += amount;
        
        // Clamp the score to not be less than 0
        score = Mathf.Clamp(score, 0, int.MaxValue);
        
        // Check if current score beats high score
        if (score > highScore)
        {
            highScore = score;
        }
    }
    
    // Add score with a text indicator
    public void AddScoreWithIndicator(int amount, string indicatorText)
    {
        AddScore(amount);
        UIDisplay display = FindObjectOfType<UIDisplay>();
        if (display != null)
        {
            display.ShowScoreIndicator(amount, indicatorText);
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
        
        AddScoreWithIndicator(trickScore, indicatorText);
        
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
        // Save high score before resetting
        SaveHighScore();
        Invoke("ResetGame", loadDelay);
    }
    
    // Load high score from saved file
    private void LoadHighScore()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            highScore = data.highScore;
        }
        else
        {
            highScore = 0;
        }
    }
    
    // Save high score to file
    private void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
    }
    
    // Reset game session to the first level
    void ResetGame()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}