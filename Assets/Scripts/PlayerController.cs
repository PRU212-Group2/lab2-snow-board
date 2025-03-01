using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    static readonly int isJumping = Animator.StringToHash("isJumping");
    
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float boostSpeed = 30f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float jumpSpeed = 30f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBoardCollider;
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoardCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            Skating();
            Falling();
        }
    }
    
    // Get input value and store in jumpInput
    void OnJump(InputValue value)
    {   
        // Check if the player is standing on the ground
        if (IsGroundTouching() && value.isPressed)
        {
            // Jump physics
            myRigidBody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }
    
    public void DisableControls()
    {
        if (!canMove) return;
        canMove = false;
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
            myAnimator.SetBool( isJumping, false);
    }
    
    void Falling()
    {
        if (!IsGroundTouching())
            myAnimator.SetBool( isJumping, true);
    }
}
