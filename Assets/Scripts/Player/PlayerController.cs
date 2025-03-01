using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    static readonly int isJumping = Animator.StringToHash("isJumping");
    private static readonly int dyingTrigger = Animator.StringToHash("crashingTrigger");
    
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float boostSpeed = 30f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float jumpSpeed = 30f;
    [SerializeField] float jumpBoost = 10f;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] ParticleSystem trickParticles;
    [SerializeField] ParticleSystem snowParticles;
    
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBoardCollider;
    GameManager gameManager;
    AudioPlayer audioPlayer;
    bool canMove = true;
    
    // Rotation tracking
    private float totalRotation = 0f;
    private float previousRotation = 0f;
    private bool isTrackingTrick = false;
    private bool isTrickCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoardCollider = GetComponent<CapsuleCollider2D>();
        gameManager = FindFirstObjectByType<GameManager>();
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
        
        // Initialize rotation value
        previousRotation = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            Skating();
            TrackRotation();
        }
    }
    
    void TrackRotation()
    {
        // Only track rotation when in the air
        if (!IsGroundTouching())
        {
            if (!isTrackingTrick)
            {
                // Start tracking a new trick
                isTrackingTrick = true;
                totalRotation = 0f;
                isTrickCompleted = false;
            }
            
            // Calculate the rotation change since last frame
            float currentRotation = transform.eulerAngles.z;
            float deltaRotation = Mathf.DeltaAngle(previousRotation, currentRotation);
            
            // Add to total rotation
            totalRotation += deltaRotation;
            
            // Check if we've completed a 360 (or multiple 360s)
            if (!isTrickCompleted && Mathf.Abs(totalRotation) >= 360f)
            {
                // We completed at least one full rotation
                CompleteRotationTrick();
                isTrickCompleted = true;
            }
            
            // Save current rotation for next frame
            previousRotation = currentRotation;
        }
        else if (isTrackingTrick)
        {
            // We've landed, stop tracking trick
            isTrackingTrick = false;
            totalRotation = 0f;
        }
    }
    
    void CompleteRotationTrick()
    {
        // How many full 360s did we complete
        int fullRotations = Mathf.FloorToInt(Mathf.Abs(totalRotation) / 360f);
        
        if (fullRotations > 0)
        {
            // Play trick effect
            if (trickParticles != null)
            {
                trickParticles.Play();
                audioPlayer.PlayBoostClip();
            }
            
            // Add score through GameManager
            gameManager.CompleteTrick();
        }
    }
    
    // Get input value and store in jumpInput
    void OnJump(InputValue value)
    {   
        myAnimator.SetBool(isJumping, value.isPressed);
        
        // Check if the player is standing on the ground
        if (IsGroundTouching() && value.isPressed)
        {
            // Jump physics
            myRigidBody.linearVelocity += new Vector2(jumpBoost, jumpSpeed);
        }
    }

    // If the player head is touching the ground then activate Crash
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Ground"))
        {
            Crash();
        }    
    }
    
    public void Crash()
    {
        if (!canMove) return;

        // Play effects and animations
        crashEffect.Play();
        audioPlayer.PlayCrashClip();
        myAnimator.SetTrigger(dyingTrigger);

        // Freeze position and rotation
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        canMove = false;
        gameManager.ProcessPlayerCrash();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    void RotatePlayer()
    {
        if (moveInput.x < 0)
        {
            rb2d.AddTorque(-moveInput.x * torqueAmount);
        }
        else if (moveInput.x > 0)
        {
            rb2d.AddTorque(moveInput.x * -torqueAmount);
        }
    }
    
    bool IsGroundTouching()
    {
        return myBoardCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
    
    void Skating()
    {
        if (IsGroundTouching())
        {
            audioPlayer.StartSnowboardingSound();
            snowParticles.Play();
        }
        else
        {
            audioPlayer.StopSnowboardingSound();
            snowParticles.Stop();
        }
    }
}