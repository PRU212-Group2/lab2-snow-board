using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float checkpointTimeAddition = 5f;
    [SerializeField] ParticleSystem checkpointParticles;

    GameManager gameManager;
    AudioPlayer audioPlayer;

    const string playerString = "Player";
    
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerString))
        {
            gameManager.IncreaseTime(checkpointTimeAddition);
            checkpointParticles.Play();
            audioPlayer.PlayBoostClip();
        }    
    }
}