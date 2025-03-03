using UnityEngine;
using UnityEngine.Serialization;

public class AudioPlayer : MonoBehaviour
{
    [Header("SnowBoarding")] 
    [SerializeField] AudioClip snowBoardingClip;
    [SerializeField] [Range(0f, 1f)] float snowBoardingVolume = 1f;
    
    [Header("Crash")]
    [SerializeField] AudioClip crashClip;
    [SerializeField] [Range(0f, 1f)] float crashVolume = 1f;
    
    [Header("Boost")]
    [SerializeField] AudioClip boostClip;
    [SerializeField] [Range(0f, 1f)] float boostVolume = 1f;
    
    [Header("Pickup")]
    [SerializeField] AudioClip pickupClip;
    [SerializeField] [Range(0f, 1f)] float pickupVolume = 1f;
    
    [Header("Songs")]
    [SerializeField] AudioClip[] songs; 
    [SerializeField] [Range(0f, 1f)] float songVolume = 1f;

    int currentSongIndex = 0;
    AudioSource audioSource;
    private AudioSource snowboardingSource;
    private bool isSnowboardingPlaying = false;
    
    static AudioPlayer _instance;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Create a dedicated audio source for snowboarding sound
        snowboardingSource = gameObject.AddComponent<AudioSource>();
        snowboardingSource.clip = snowBoardingClip;
        snowboardingSource.volume = snowBoardingVolume;
        snowboardingSource.loop = true;
        
        // Play the first song on start
        if (songs.Length > 0)
        {
            PlayCurrentSong();
        }
    }

    void Awake()
    {
        ManageSingleton();
    }
    
    void Update()
    {
        // Continuously check if the song is playing and switch to the next one
        CheckAndPlayNextSong();
    }

    void PlayCurrentSong()
    {
        // Set the current song and play it
        audioSource.clip = songs[currentSongIndex];
        audioSource.volume = songVolume;
        audioSource.Play();
    }

    void CheckAndPlayNextSong()
    {
        // Check if the current song is finished playing
        if (!audioSource.isPlaying)
        {
            // Move to the next song and play it
            currentSongIndex = (currentSongIndex + 1) % songs.Length;
            PlayCurrentSong();
        }
    }

    // Applying singleton pattern
    void ManageSingleton()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartSnowboardingSound()
    {
        if (!isSnowboardingPlaying && snowboardingSource != null)
        {
            snowboardingSource.Play();
            isSnowboardingPlaying = true;
        }
    }
    
    public void StopSnowboardingSound()
    {
        if (isSnowboardingPlaying && snowboardingSource != null)
        {
            snowboardingSource.Stop();
            isSnowboardingPlaying = false;
        }
    }

    public void PlayCrashClip()
    {
        PlayClip(crashClip, crashVolume);
    }
    
    public void PlayBoostClip()
    {
        PlayClip(boostClip, boostVolume);
    }
    
    public void PlayPickupClip()
    {
        PlayClip(pickupClip, pickupVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (clip)
        {
            // Play audio clip at camera with defined volume
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
