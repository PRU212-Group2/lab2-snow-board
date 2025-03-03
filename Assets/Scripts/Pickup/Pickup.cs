using UnityEngine;
using UnityEngine.Serialization;

public class Pickup : MonoBehaviour
{
    [SerializeField] ParticleSystem pickupParticle;
    [SerializeField] int pickupPoint = 50;
    
    AudioPlayer audioPlayer;
    ScoreManager scoreManager;

    void Awake()
    {
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides then triggers effects
        if (other.CompareTag("Player"))
        {
            Instantiate(pickupParticle, transform.position, Quaternion.identity);
            audioPlayer.PlayPickupClip();
            scoreManager.AddScore(pickupPoint);
            Destroy(gameObject);
        }
    }
}
