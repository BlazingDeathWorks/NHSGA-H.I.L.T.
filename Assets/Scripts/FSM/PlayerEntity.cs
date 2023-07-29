using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public FiniteStateMachine FiniteStateMachine { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    //Player Movement
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; set; }
    public bool IsFalling { get; private set; }
    public bool IsGrounded { get; private set; } = true;
    public bool FinishedAttacking { get; set; }
    public bool IsCoyoteTime { get; set; }
    public float TimeSinceStartFall { get; set; }
    public float JumpPower => jumpPower;
    public float PlatformRaycastDistance => platformRaycastDistance;
    public float CoyoteTime => coyoteTime;
    public Transform[] GroundRaycastPositions => groundRaycastPositions;
    public Transform[] PlatformRaycastPositions => platformRaycastPositions;
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpPower = 3;
    [SerializeField] private float jumpBuffer = 0.45f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private Transform[] groundRaycastPositions;
    [SerializeField] private Transform[] platformRaycastPositions;
    [SerializeField] private float groundRaycastDistance = 0.2f;
    [SerializeField] private float platformRaycastDistance = 1;
    private float x;
    private float timeSinceJumpPressed;

    //Player Animation States (STEP #1)
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerRunState PlayerRunState { get; private set; }
    public PlayerJumpState PlayerJumpState { get; private set; }
    public PlayerFallState PlayerFallState { get; private set; }
    public PlayerBaseAttackState PlayerBaseAttackState { get; private set; }
    public PlayerBaseAirAttackState PlayerBaseAirAttackState { get; private set; }
    public PlayerThreeHitAttackState PlayerThreeHitAttackState { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        timeSinceJumpPressed = jumpBuffer;

        FiniteStateMachine = new FiniteStateMachine();

        //Initialize Player Animation States (STEP #2)
        PlayerIdleState = new PlayerIdleState(this, FiniteStateMachine);
        PlayerRunState = new PlayerRunState(this, FiniteStateMachine);
        PlayerJumpState = new PlayerJumpState(this, FiniteStateMachine);
        PlayerFallState = new PlayerFallState(this, FiniteStateMachine);
        PlayerBaseAttackState = new PlayerBaseAttackState(this, FiniteStateMachine);
        PlayerBaseAirAttackState = new PlayerBaseAirAttackState(this, FiniteStateMachine);
        PlayerThreeHitAttackState = new PlayerThreeHitAttackState(this, FiniteStateMachine);

        //Initialize Current State
        FiniteStateMachine.Initialize(PlayerIdleState);
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (x != 0)
        {
            IsRunning = true;
            FlipPlayer();
        }
        else
        {
            IsRunning = false;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || timeSinceJumpPressed < jumpBuffer) && IsGrounded)
        {
            IsJumping = true;
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !IsGrounded && IsFalling)
        {
            timeSinceJumpPressed = 0;
        }
        timeSinceJumpPressed += Time.deltaTime;

        FiniteStateMachine.CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < groundRaycastPositions.Length; i++)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(groundRaycastPositions[i].position, Vector2.down, groundRaycastDistance);
            if (hitInfo && (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Platform")))
            {
                IsGrounded = true;
                break;
            }
            IsGrounded = false;
        }

        Rb.velocity = new Vector2(x * speed, Rb.velocity.y);

        IsFalling = Rb.velocity.y < -0.2f;

        FiniteStateMachine.CurrentState.OnFixedUpdate();
    }

    private void OnDrawGizmos()
    {
        if (groundRaycastPositions == null || groundRaycastPositions.Length <= 0) return;
        for (int i = 0; i < groundRaycastPositions.Length; i++)
        {
            Gizmos.DrawLine(groundRaycastPositions[i].position, (Vector2)groundRaycastPositions[i].position + Vector2.down * groundRaycastDistance);
        }

        if (platformRaycastPositions == null || platformRaycastPositions.Length <= 0) return;
        for (int i = 0; i < platformRaycastPositions.Length; i++)
        {
            Gizmos.DrawLine(platformRaycastPositions[i].position, (Vector2)platformRaycastPositions[i].position + Vector2.up * platformRaycastDistance);
        }
    }

    private void FlipPlayer()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(x), transform.localScale.y, transform.localScale.z);
    }

    public void FinishAttack()
    {
        FinishedAttacking = true;
    }
}
