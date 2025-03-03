using UnityEngine;
using UnityEngine.Serialization;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] int startingDifficultyDistanceThreshold = 1000;
    [SerializeField] int distanceThresholdIncreaseAmount = 1000;
    [SerializeField] float chunkMoveSpeedIncreaseAmount = 1f;

    [SerializeField] float fogDensityIncreaseAmount = 0.005f;
    [SerializeField] float maxFogDensity = 0.05f;
    [SerializeField] int fogScoreThreshold = 5000;

    ScoreManager scoreManager;
    LevelGenerator levelGenerator;

    void Awake()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        
        // Enable fog at the start if necessary
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.01f; // Initial fog density
    }
    
    void Update()
    {
        CheckDifficultyThreshold();
    }

    private void CheckDifficultyThreshold()
    {
        int distanceTraveled = scoreManager.GetDistanceTraveled();

        if (distanceTraveled >= startingDifficultyDistanceThreshold)
        {
            ChangeDifficulty();
            startingDifficultyDistanceThreshold += distanceThresholdIncreaseAmount;
        }

        // Check if fog intensity needs to increase
        if (distanceTraveled >= fogScoreThreshold)
        {
            IncreaseFogDensity();
            fogScoreThreshold += distanceThresholdIncreaseAmount;
        }
    }

    private void ChangeDifficulty()
    {
        levelGenerator.ChangeChunkMoveSpeed(chunkMoveSpeedIncreaseAmount);
    }

    private void IncreaseFogDensity()
    {
        if (RenderSettings.fogDensity < maxFogDensity)
        {
            RenderSettings.fogDensity += fogDensityIncreaseAmount;
            RenderSettings.fogDensity = Mathf.Min(RenderSettings.fogDensity, maxFogDensity);
        }
    }
}
