using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIGameOver : MonoBehaviour
{
    private GameManager gameManager;
    private ScoreManager scoreManager;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button restartButton;
    public Button mainMenuButton;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        scoreManager = FindFirstObjectByType<ScoreManager>();

        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }


    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        scoreText.text = "Score: " + scoreManager.GetScore() + "\nHighest Score: " + scoreManager.GetHighScore();
        scoreText.text = "\nDistance: " + scoreManager.GetDistanceTraveled() + "\nHighest Distance: " + scoreManager.GetFurthestDistanceTraveled();
        gameOverText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        var mode = gameManager.GetGameMode();
        if (mode == "Normal")
        {
            SceneManager.LoadScene("Normal");
        }
        else
        {
            SceneManager.LoadScene("Time Trial");
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
