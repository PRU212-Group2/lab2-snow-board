using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI furthestDistanceText;

    ScoreManager scoreManager;
    GameManager gameManager; 

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        highScoreText.text = scoreManager.GetHighScore().ToString("0000000");
        furthestDistanceText.text = scoreManager.GetFurthestDistanceTraveled().ToString("0000000");
    }
    
    public void OnStartPressed()
    {
        gameManager.SetGameMode("Normal");
        gameManager.ResetGame();
    }
    
    public void OnTimeTrialPressed()
    {
        gameManager.SetGameMode("Time Trial");
        gameManager.ResetGame();
    }
    
    public void OnHelpPressed()
    {
        gameManager.HelpMenu();
    }
    
    public void OnQuitPressed()
    {
        gameManager.QuitGame();
    }
}
