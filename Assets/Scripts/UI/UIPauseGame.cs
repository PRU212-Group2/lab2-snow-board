using UnityEngine;

public class UIPauseGame : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    
    float previousTimeScale = 1;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void OnHomePressed()
    {
        gameManager.MainMenu();
    }
    
    public void OnResumePressed()
    {
        TogglePause();
    }

    public void OnRestartPressed()
    {
        TogglePause();
        gameManager.ResetGame();
    }
    
    void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            pauseMenu.SetActive(false);
        }
    }
}
