using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering.Universal;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int progressMeterPerSecond = 10;
    [SerializeField] int trickBaseScore = 300;
    [SerializeField] float comboTimeWindow = 5f;
    [SerializeField] int maxSkillChained = 5;
    
    private float lastDistanceUpdateTime;
    private float lastTrickTime;
    private int comboCounter = 0;
    private int distanceTraveled = 0;
    private int furthestDistanceTraveled = 0;
    private int score;
    private int highScore;
    
    private string saveFilePath;
    
    static ScoreManager _instance;
    
    void Awake()
    {
        ManageSingleton();
        // Set the save file path
        saveFilePath = Path.Combine(Application.persistentDataPath, "highscore.json");
        LoadHighScore();
    }
    
    void Start()
    {
        lastDistanceUpdateTime = Time.time;
    }
    
    void Update()
    {
        DistanceUpdate();
        ComboTracker();
    }

    private void DistanceUpdate()
    {
        // Add progressive distance over time
        if (Time.time - lastDistanceUpdateTime >= 1f)
        {
            AddDistance(progressMeterPerSecond);
            lastDistanceUpdateTime = Time.time;
        }
    }

    private void ComboTracker()
    {
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
    
    // Public method to retrieve distance
    public int GetDistanceTraveled()
    {
        return distanceTraveled;
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
    
    // Add distance traveled based on events
    public void AddDistance(int meters)
    {
        distanceTraveled += meters;
        
        // Clamp the score to not be less than 0
        distanceTraveled = Mathf.Clamp(distanceTraveled, 0, int.MaxValue);
        
        // Check if current score beats high score
        if (distanceTraveled > furthestDistanceTraveled)
        {
            furthestDistanceTraveled = distanceTraveled;
        }
    }

    public int GetFurthestDistanceTraveled()
    {
        return furthestDistanceTraveled;
    }
    
    // Add score with a text indicator
    public void AddScoreWithIndicator(int amount, string indicatorText)
    {
        AddScore(amount);
        UIDisplay display = FindFirstObjectByType<UIDisplay>();
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
        distanceTraveled = 0;
        SaveHighScore();
    }
    
    // Load high score from saved file
    private void LoadHighScore()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            highScore = data.highScore;
            furthestDistanceTraveled = data.furthestDistanceTraveled;
        }
        else
        {
            highScore = 0;
            furthestDistanceTraveled = 0;
        }
    }
    
    // Save high score to file
    private void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.furthestDistanceTraveled = furthestDistanceTraveled;
        
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
    }
}