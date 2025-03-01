using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    
    private string saveFilePath;
    
    ScoreManager scoreManager;
    static GameManager _instance;
    
    void Awake()
    {
        ManageSingleton();
        scoreManager = FindFirstObjectByType<ScoreManager>();
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
        scoreManager.ResetScore();
        SceneManager.LoadScene(0);
    }
}