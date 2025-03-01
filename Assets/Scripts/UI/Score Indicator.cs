using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] float fadeTime = 1.5f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] Vector3 moveDirection = Vector3.up;
    
    private TextMeshProUGUI textComponent;
    
    public void Initialize(int score, string text)
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = $"+{score}\n{text}";
        
        StartCoroutine(AnimateAndDestroy());
    }
    
    private IEnumerator AnimateAndDestroy()
    {
        float startTime = Time.time;
        Color startColor = textComponent.color;
        Vector3 startPosition = transform.position;
        
        while (Time.time - startTime < fadeTime)
        {
            float progress = (Time.time - startTime) / fadeTime;
            
            // Move upward
            transform.position = startPosition + moveDirection * moveSpeed * progress;
            
            // Fade out
            Color newColor = startColor;
            newColor.a = 1 - progress;
            textComponent.color = newColor;
            
            yield return null;
        }
        
        Destroy(gameObject);
    }
}