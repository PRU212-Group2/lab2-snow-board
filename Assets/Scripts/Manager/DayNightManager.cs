using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    [Header("Day/Night Cycle")]
    [SerializeField] int dayNightTransitionScore = 3000;
    [SerializeField] int nightDayTransitionScore = 1500;
    [SerializeField] Light2D globalLight;
    [SerializeField] float transitionDuration = 5f;
    [SerializeField] Color dayColor = Color.white;
    [SerializeField] Color nightColor = new Color(0.18f, 0.19f, 0.24f, 1f); // Fixed color format
    [SerializeField] float dayIntensity = 1f;
    [SerializeField] float nightIntensity = 0.5f;
    [SerializeField] bool enableCyclicalDayNight = true;
    
    [Header("Sun/Moon Settings")]
    [SerializeField] GameObject sunPrefab;
    [SerializeField] GameObject moonPrefab;
    [SerializeField] Transform celestialParent;
    [SerializeField] Vector2 celestialPosition = new Vector2(0f, 0f);
    [SerializeField] float celestialTransitionSpeed = 2f;
    
    private bool isDayNightTransitioning = false;
    private float transitionStartTime;
    private bool isNightTime = false;
    private int lastCycleScore = 0;
    private GameObject currentSun;
    private GameObject currentMoon;
    private float celestialAlpha = 1f;
    ScoreManager scoreManager;
    
    // Components for fading
    private SpriteRenderer sunRenderer;
    private SpriteRenderer moonRenderer;
    
    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        
        // Initialize global light
        InitializeGlobalLight();

        // Initialize celestial objects
        InitializeCelestials();
    }

    private void InitializeGlobalLight()
    {
        // Find the global light if not assigned
        if (globalLight == null)
        {
            // Try to find a global light in the scene
            Light2D[] lights = FindObjectsOfType<Light2D>();
            foreach (Light2D light in lights)
            {
                // Look for a global light with the highest intensity
                if (light.lightType == Light2D.LightType.Global)
                {
                    if (globalLight == null || light.intensity > globalLight.intensity)
                    {
                        globalLight = light;
                    }
                }
            }
            
            if (globalLight == null)
            {
                Debug.LogWarning("No global Light2D found for day/night cycle. Please assign one in the inspector.");
            }
        }
    }

    void Update()
    {
        HandleDayNightTransition();
        UpdateCelestialObjects();
    }
    
    private void InitializeCelestials()
    {
        // Create parent if specified but doesn't exist
        if (celestialParent == null)
        {
            GameObject parent = new GameObject("CelestialObjects");
            celestialParent = parent.transform;
        }
    
        // Create sun if prefab exists
        if (sunPrefab != null && currentSun == null)
        {
            // Instantiate at zero position (world space)
            currentSun = Instantiate(sunPrefab, Vector3.zero, Quaternion.identity, celestialParent);
            // Then set local position
            currentSun.transform.localPosition = celestialPosition;
        
            sunRenderer = currentSun.GetComponent<SpriteRenderer>();
            if (sunRenderer == null)
            {
                sunRenderer = currentSun.AddComponent<SpriteRenderer>();
                Debug.LogWarning("Sun prefab has no SpriteRenderer. Adding one automatically.");
            }
        }
    
        // Create moon if prefab exists
        if (moonPrefab != null && currentMoon == null)
        {
            // Instantiate at zero position (world space)
            currentMoon = Instantiate(moonPrefab, Vector3.zero, Quaternion.identity, celestialParent);
            // Then set local position
            currentMoon.transform.localPosition = celestialPosition;
            
            moonRenderer = currentMoon.GetComponent<SpriteRenderer>();
            
            // Add a light to the moon (only if it doesn't already have one)
            Light2D moonLight = currentMoon.GetComponent<Light2D>();
            if (moonLight == null)
            {
                moonLight = currentMoon.AddComponent<Light2D>();
                moonLight.lightType = Light2D.LightType.Point;
                moonLight.intensity = 1f;
                moonLight.color = new Color(0.9f, 0.95f, 1f, 1f);
                moonLight.pointLightOuterRadius = 5f;
            }
            
            // Hide moon initially
            SetCelestialAlpha(moonRenderer, 0);
        }
    }
    
    private void UpdateCelestialObjects()
    {
        if (sunRenderer == null || moonRenderer == null) return;
        
        if (isDayNightTransitioning)
        {
            float timeSinceStart = Time.time - transitionStartTime;
            float progress = Mathf.Clamp01(timeSinceStart / transitionDuration);
            
            if (!isNightTime) // Transitioning to night
            {
                // Fade out sun, fade in moon
                SetCelestialAlpha(sunRenderer, 1 - progress);
                SetCelestialAlpha(moonRenderer, progress);
            }
            else // Transitioning to day
            {
                // Fade in sun, fade out moon
                SetCelestialAlpha(sunRenderer, progress);
                SetCelestialAlpha(moonRenderer, 1 - progress);
            }
        }
    }
    
    private void SetCelestialAlpha(SpriteRenderer renderer, float alpha)
    {
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }

    private void HandleDayNightTransition()
    {
        int distanceTraveled = scoreManager.GetDistanceTraveled();

        // Handle transitions
        if (isDayNightTransitioning)
        {
            UpdateDayNightTransition();
        }
        else if (!isNightTime && distanceTraveled >= dayNightTransitionScore && 
                 (distanceTraveled - lastCycleScore >= dayNightTransitionScore || lastCycleScore == 0))
        {
            // Transition to night when first reaching threshold or after completing a full cycle
            StartDayToNightTransition();
            lastCycleScore = distanceTraveled;
        }
        else if (isNightTime && enableCyclicalDayNight && 
                 distanceTraveled >= lastCycleScore + nightDayTransitionScore)
        {
            // Transition back to day after reaching the next threshold
            StartNightToDayTransition();
            lastCycleScore = distanceTraveled;
        }
    }

    // Start transitioning to night time
    private void StartDayToNightTransition()
    {
        if (globalLight != null)
        {
            isDayNightTransitioning = true;
            transitionStartTime = Time.time;
            isNightTime = false;
        }
    }
    
    // Start transitioning to day time
    private void StartNightToDayTransition()
    {
        if (globalLight != null)
        {
            isDayNightTransitioning = true;
            transitionStartTime = Time.time;
            isNightTime = true;
        }
    }
    
    // Update the transition effect
    private void UpdateDayNightTransition()
    {
        if (globalLight == null) return;
        
        float timeSinceStart = Time.time - transitionStartTime;
        float progress = Mathf.Clamp01(timeSinceStart / transitionDuration);
        
        if (!isNightTime)
        {
            // Update light color and intensity
            globalLight.color = Color.Lerp(dayColor, nightColor, progress);
            globalLight.intensity = Mathf.Lerp(dayIntensity, nightIntensity, progress);
        }
        else
        {
            // Update light color and intensity (reverse direction)
            globalLight.color = Color.Lerp(nightColor, dayColor, progress);
            globalLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, progress);
        }
        
        // Check if transition is complete
        if (progress >= 1.0f)
        {
            isDayNightTransitioning = false;
            isNightTime = !isNightTime;
        }
    }
    
    // Reset day/night cycle when starting a new game
    public void ResetDayNightCycle()
    {
        if (globalLight != null)
        {
            globalLight.color = dayColor;
            globalLight.intensity = dayIntensity;
            isNightTime = false;
            isDayNightTransitioning = false;
            lastCycleScore = 0;
            
            // Reset celestial objects
            if (sunRenderer != null && moonRenderer != null)
            {
                SetCelestialAlpha(sunRenderer, 1);
                SetCelestialAlpha(moonRenderer, 0);
            }
        }
    }
}