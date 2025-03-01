using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDisplay : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject scoreIndicatorPrefab;
    [SerializeField] RectTransform scoreIndicatorParent;
    [SerializeField] Vector2 screenOffset = new Vector2(0, 50);
    
    GameManager scoreManager;

    void Awake()
    {
        scoreManager = FindFirstObjectByType<GameManager>();
        
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
    }

    void Update()
    {
        // Update score text on the UI
        scoreText.text = scoreManager.GetHighScore().ToString("00000");
    }
    
    public void ShowScoreIndicator(int score, string text)
    {
        if (scoreIndicatorPrefab == null || scoreIndicatorParent == null)
            return;
        
        // Simply instantiate the indicator as a child of the existing parent
        GameObject indicator = Instantiate(scoreIndicatorPrefab, scoreIndicatorParent);
    
        // You can position it relative to the player's screen position if needed
        // If you want it at a fixed position, you can set it directly
    
        // Initialize the score indicator with the appropriate text
        ScoreIndicator scoreIndicator = indicator.GetComponent<ScoreIndicator>();
        if (scoreIndicator != null)
        {
            scoreIndicator.Initialize(score, text);
        }
    }
}