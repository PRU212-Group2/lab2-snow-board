using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    CapsuleCollider2D bodyCollider;
    BoxCollider2D topCollider;
    AudioPlayer audioPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        bodyCollider = GetComponent<CapsuleCollider2D>();
        topCollider = GetComponent<BoxCollider2D>();
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
    }

    void Update()
    {
        bool isTouchingBody = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        bool isTouchingTop = topCollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        
        if (isTouchingBody)
        {
            var player = FindFirstObjectByType<PlayerController>();
            player.Crash();
            Invoke("ReloadScene", 2);
        }
        else if (isTouchingTop)
        {
            audioPlayer.PlayBoostClip();   
        }
    }
    
    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
