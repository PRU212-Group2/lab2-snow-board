using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI furthestDistanceText;

    ScoreManager scoreManager;
    GameManager gameManager; 

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        scoreText.text = scoreManager.GetScore().ToString("0000000");
        highScoreText.text = scoreManager.GetHighScore().ToString("0000000");
        distanceText.text = scoreManager.GetDistanceTraveled().ToString("0000000");
        furthestDistanceText.text = scoreManager.GetFurthestDistanceTraveled().ToString("0000000");
    }
    
    public void OnRestartPressed()
    {
        gameManager.ResetGame();
    }

    public void OnMainMenuPressed()
    {
        gameManager.MainMenu();
    }
    
    public void OnQuitPressed()
    {
        gameManager.QuitGame();
    }
}
