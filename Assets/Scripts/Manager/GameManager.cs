using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    [SerializeField] float startTime = 60f;
    [SerializeField] string gameMode = "Normal";
    
    ScoreManager scoreManager;
    DayNightManager dayNightManager;
    private float timeLeft;
    static GameManager _instance;
    
    void Start()
    {
        timeLeft = startTime;
    }
    
    void Awake()
    {
        ManageSingleton();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        dayNightManager = FindFirstObjectByType<DayNightManager>();
    }
    
    void Update()
    {
        if (gameMode.Equals("Time Trial")) DecreaseTime();
    }

    public void SetGameMode(string mode)
    {
        gameMode = mode;
    }
    
    public void IncreaseTime(float amount)
    {
        timeLeft += amount;
    }

    void DecreaseTime()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            Invoke("ResetGame", loadDelay);
        }
    }

    public float GetTime()
    {
        return timeLeft;
    }

    public string GetGameMode()
    {
        return gameMode;
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
    
    // Either reset the current level or the whole game session
    public void ProcessPlayerCrash()
    {
        Invoke("ResetGame", loadDelay);
    }
    
    // Reset game session to the first level
    public void ResetGame()
    {
        timeLeft = startTime;
        dayNightManager.ResetDayNightCycle();
        scoreManager.ResetScore();
        SceneManager.LoadScene("Endless Runner");
    }

    // Load Main menu scene
    public void MainMenu()
    {
        timeLeft = startTime;
        scoreManager.ResetScore();
        SceneManager.LoadScene("MainMenu");
    }
}