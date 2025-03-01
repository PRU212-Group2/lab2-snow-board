using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    [SerializeField] float startTime = 60f;
    [SerializeField] string gameMode = "Normal";
    
    ScoreManager scoreManager;
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
    }
    
    void Update()
    {
        if (gameMode.Equals("Time Trial"))
        {
            DecreaseTime();
        }
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
    void ResetGame()
    {
        timeLeft = startTime;
        scoreManager.ResetScore();
        SceneManager.LoadScene(0);
    }
}