using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //0 is ranged
    public int aiType;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private LayerMask tileMask;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject projectileOrigin;
    [SerializeField]
    private float aggroFollowDistance;
    [SerializeField]
    private float aggroDistance;

    private GameObject player;
    private Collider2D playerCollider;
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D rb;
    private bool active;
    private float dir;
    private float leftBound;
    private float rightBound;
    private State state;
    private float aggroTime;
    private float nextAttack;
    private float aggroGoalX;

    public enum State
    {
        idle, aggro, attacking
    };
    void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        active = false;
        dir = Random.Range(0, 2) * 2 - 1;
        leftBound = rightBound = -100;
    }

    void Update()
    {
        if (!active && (player.transform.position - transform.position).magnitude < 50f)
        {
            active = true;
        }
        if (active)
        {
            switch (state)
            {
                case State.idle:
                    Patrol();
                    FindPlayer(true);
                    break;

                case State.aggro:
                    switch (aiType)
                    {
                        case 0:
                            float thisX = transform.position.x;
                            float playerX = player.transform.position.x;
                            if(Mathf.Abs(aggroGoalX-thisX) > .1f)
                            {
                                dir = Mathf.Sign(aggroGoalX - thisX);
                                rb.velocity = Vector3.right * speed * dir / 2f;
                            } else
                            {
                                dir = Mathf.Sign(playerX - thisX);
                            }
                            if(Time.time > aggroTime)
                            {
                                state = State.idle;
                                anim.SetTrigger("idle");
                            }
                            if (Time.time > nextAttack)
                            {
                                dir = Mathf.Sign(playerX - thisX);
                                anim.SetTrigger("attack");
                                state = State.attacking;
                            }
                            break;
                    }
                    FindPlayer(false);
                    break;

                case State.attacking:
                    switch (aiType)
                    {
                        case 0:
                            Instantiate(projectilePrefab, projectileOrigin.transform.position, 
                                dir > 0 ? Quaternion.identity : Quaternion.Euler(0, 0, 180));
                            state = State.aggro;
                            break;
                    }
                    break;
            }
        }
        projectileOrigin.transform.localPosition = new Vector3(Mathf.Abs(projectileOrigin.transform.localPosition.x) * 
            dir < 0 ? -1 : 1, projectileOrigin.transform.localPosition.y);
        sprite.flipX = dir < 0;
    }

    private void FindPlayer(bool ShouldChangeState)
    {
        bool canSeePlayer = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right * dir, aggroDistance).collider.Equals(playerCollider);
        if (canSeePlayer)
        {
            aggroTime = Time.time + 1.5f;
            if(nextAttack < Time.time)
            {
                aggroGoalX = Mathf.Clamp(player.transform.position.x + 
                    aggroFollowDistance * (transform.position.x < player.transform.position.x ? -1 : 1), leftBound, rightBound);
                nextAttack = Time.time + attackCooldown;
            }
            if (ShouldChangeState)
            {
                anim.SetTrigger("aggro");
                state = State.aggro;
            }
        }

    }

    private void Patrol()
    {
        rb.velocity = Vector3.right * speed * dir;
        //set patrol bounds
        if (leftBound == -100 && dir < 0)
        {
            bool isStopped = !Physics2D.OverlapCircle(transform.position + Vector3.left * 2, .1f, tileMask) ||
                Physics2D.OverlapPoint(transform.position + new Vector3(-2, .5f), tileMask);
            if (isStopped)
            {
                leftBound = transform.position.x;
            }
        }
        if (rightBound == -100 && dir > 0)
        {
            bool isStopped = !Physics2D.OverlapCircle(transform.position + Vector3.right * 2, .1f, tileMask) ||
                Physics2D.OverlapPoint(transform.position + new Vector3(2, .5f), tileMask);
            if (isStopped)
            {
                rightBound = transform.position.x;
            }
        }

        //flip at bounds
        if (leftBound != -100 && transform.position.x < leftBound) dir = 1;
        if (rightBound != -100 && transform.position.x > rightBound) dir = -1;
    }
}
