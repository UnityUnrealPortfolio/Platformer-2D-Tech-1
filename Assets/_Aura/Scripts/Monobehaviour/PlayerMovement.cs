using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlatformerActions;//imports all static and nested types from a single type(PlatformerActions)

public class PlayerMovement : MonoBehaviour,IPlayerActions
{
    //public fields
    [Header("Movement Properties")] 
    [Tooltip("How fast player runs")][SerializeField] float moveSpeed;
    [Tooltip("Speed threshold above which to detect sideways movement")]
    [SerializeField] float sideSpeedThreshold;
    [Tooltip("How high the player jumps")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;



    //private fields
    Animator playerAnimator;
    PlatformerActions actions;
    Rigidbody2D playerRb;
    CapsuleCollider2D playerCollider;
    Vector2 moveInput = Vector2.zero;
    float startGravityScale;

    //Animator hashes
    int isRunningHash = Animator.StringToHash("isRunning");
    int isClimbingHash = Animator.StringToHash("isClimbing");
    
    #region Monobehaviour Callbacks
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        startGravityScale = playerRb.gravityScale;
    }
    private void OnEnable()
    {
        actions = new PlatformerActions();
        actions.Player.AddCallbacks(this);
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void FixedUpdate()
    {
        Run();
        FlipPlayer();
        Climb();
    }

    private void Climb()
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
         
            playerRb.gravityScale = startGravityScale;
            playerAnimator.SetBool(isClimbingHash, false);

            return;
        }
        else
        {

           
            //cached into locals for readability and teaching purposes
            float horizontalMoveInput = playerRb.velocity.x;
            float verticalMoveInput = moveInput.y * climbSpeed;


            //construct new vector 
            Vector2 moveVector = new Vector2(horizontalMoveInput, verticalMoveInput);

            playerRb.velocity = moveVector;
            playerAnimator.SetBool(isClimbingHash, MathF.Abs(playerRb.velocity.y) > Mathf.Epsilon);

            //avoid player slowly sliding down while idling on the ladder
            playerRb.gravityScale = 0f;
        }

    }

    private void Run()
    {
        //cached into locals for readability and teaching purposes
        float horizontalMoveInput = moveInput.x * moveSpeed;
        float verticalMoveInput = playerRb.velocity.y;

        //construct new vector 
        Vector2 moveVector = new Vector2(horizontalMoveInput, verticalMoveInput);

        playerRb.velocity = moveVector;

        //set animator isRunning property based on velocity to true or false
        playerAnimator.SetBool(isRunningHash, Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon+sideSpeedThreshold);
    }
    #endregion

    #region Movement Utility
    private void FlipPlayer()
    {
        //only flip player if player is moving
        if (Mathf.Abs(moveInput.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }
    } 
    #endregion

    #region Input Callbacks
    public void OnJump(InputAction.CallbackContext context)
    {
        //dealing with double jump
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if(context.action.IsPressed())
        {
            playerRb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
     moveInput = context.ReadValue<Vector2>();
     Debug.Log($"Incoming moveInput {moveInput}");
    } 
    #endregion
}
