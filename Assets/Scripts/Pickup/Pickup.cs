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
        // Get current rotation
        Vector3 currentRotation = transform.eulerAngles;

        // Increment the Y-axis rotation
        currentRotation.y += 90 * Time.deltaTime;

        // Apply the new rotation
        transform.eulerAngles = currentRotation;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides then triggers effects
        if (other.CompareTag("Player"))
        {
            ParticleSystem particles = Instantiate(pickupParticle, transform.position, Quaternion.identity);
            audioPlayer.PlayPickupClip();
            scoreManager.AddScore(pickupPoint);
            Destroy(particles.gameObject, particles.main.duration);
            Destroy(gameObject);
        }
    }
}
