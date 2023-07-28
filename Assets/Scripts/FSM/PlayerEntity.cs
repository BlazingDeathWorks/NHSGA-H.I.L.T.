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
    public bool IsJumping { get; private set; }
    public bool IsFalling { get; private set; }
    public bool IsGrounded { get; private set; } = true;
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform[] groundRaycastPositions;
    [SerializeField] private float raycastDistance = 0.2f;
    private float x, y;

    //Player Animation States

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        FiniteStateMachine = new FiniteStateMachine();
        //Initialize Player Animation States
        //FiniteStateMachine.Initialize();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (x != 0)
        {
            IsRunning = true;
            FlipPlayer();
        }
        else
        {
            IsRunning = false;
        }

        if (y == 1 && IsGrounded)
        {
            IsJumping = true;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < groundRaycastPositions.Length; i++)
        {
            if (Physics2D.Raycast(groundRaycastPositions[i].position, Vector2.down, raycastDistance, 1 << LayerMask.NameToLayer("Ground")))
            {
                IsGrounded = true;
                break;
            }
            IsGrounded = false;
        }

        Rb.velocity = new Vector2(x * speed, Rb.velocity.y);

        IsFalling = Rb.velocity.y < -0.2f;
    }

    private void OnDrawGizmos()
    {
        if (groundRaycastPositions == null || groundRaycastPositions.Length <= 0) return;
        for (int i = 0; i < groundRaycastPositions.Length; i++)
        {
            Gizmos.DrawLine(groundRaycastPositions[i].position, (Vector2)groundRaycastPositions[i].position + Vector2.down * raycastDistance);
        }
    }

    private void FlipPlayer()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(x), transform.localScale.y, transform.localScale.z);
    }


}
