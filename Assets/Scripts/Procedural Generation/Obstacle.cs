using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int bounceScore = 100;
    
    CapsuleCollider2D bodyCollider;
    BoxCollider2D topCollider;
    AudioPlayer audioPlayer;
    GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        bodyCollider = GetComponent<CapsuleCollider2D>();
        topCollider = GetComponent<BoxCollider2D>();
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        bool isTouchingBody = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        bool isTouchingTop = topCollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        
        if (isTouchingBody)
        {
            var player = FindFirstObjectByType<PlayerController>();
            player.Crash();
            gameManager.ResetScore();
            gameManager.ProcessPlayerCrash();
        }
        else if (isTouchingTop)
        {
            // If the player board collide on top of
            // the obstacle then bounce and add points
            audioPlayer.PlayBoostClip();
            BounceOnObstacle();
        }
    }
    
    // Handle bounce on obstacle
    private void BounceOnObstacle()
    {
        string indicatorText = "Bounce on Rock";
        
        gameManager.AddScoreWithIndicator(bounceScore, indicatorText);
    }
}
