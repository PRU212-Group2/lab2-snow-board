using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDisplay : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject scoreIndicatorPrefab;
    [SerializeField] RectTransform scoreIndicatorParent;
    [SerializeField] TextMeshProUGUI timeleftPrefab;

    private ScoreManager scoreManager;
    private GameManager gameManager;
    private bool isTimeTrialMode = false;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        
        // Create parent for indicators if not assigned
        if (scoreIndicatorParent == null)
        {
            GameObject parent = new GameObject("ScoreIndicators");
            parent.transform.SetParent(transform);
            scoreIndicatorParent = parent.AddComponent<RectTransform>();
            scoreIndicatorParent.anchorMin = Vector2.zero;
            scoreIndicatorParent.anchorMax = Vector2.one;
            scoreIndicatorParent.offsetMin = Vector2.zero;
            scoreIndicatorParent.offsetMax = Vector2.zero;
        }
        
        CheckGameMode();
    }
    
    void Update()
    {
        // Update score text on the UI
        if (scoreText != null && scoreManager != null)
        {
            scoreText.text = scoreManager.GetScore().ToString("0000000");
        }
        
        // Update score text on the UI
        if (distanceText != null && scoreManager != null)
        {
            distanceText.text = scoreManager.GetDistanceTraveled().ToString("00000") + "m";
        }
        
        // Only Update time if in time trial mode
        if (isTimeTrialMode)
        {
            timeleftPrefab.text = gameManager.GetTime().ToString("00.0") + "s";
        }
    }
    
    public void ShowScoreIndicator(int score, string text)
    {
        if (scoreIndicatorPrefab == null || scoreIndicatorParent == null)
            return;
        
        // Simply instantiate the indicator as a child of the existing parent
        GameObject indicator = Instantiate(scoreIndicatorPrefab, scoreIndicatorParent);
    
        // Initialize the score indicator with the appropriate text
        ScoreIndicator scoreIndicator = indicator.GetComponent<ScoreIndicator>();
        if (scoreIndicator != null)
        {
            scoreIndicator.Initialize(score, text);
        }
    }
    
    private void CheckGameMode()
    {
        if (timeleftPrefab == null)
            return;
        
        isTimeTrialMode = gameManager.GetGameMode().Equals("Time Trial");
        
        // Enable or disable the timer based on game mode
        timeleftPrefab.gameObject.SetActive(isTimeTrialMode);
    }
}