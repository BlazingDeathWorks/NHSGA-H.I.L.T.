using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private float jumpHeight;

    public enum State
    {
        inactive, idle, jumping, hand, bomb, laser, defeated
    };

    private State state;
    private GameObject player;
    private float nextAttack;
    private Rigidbody2D rb;
    private float arenaX;
    private float goalPos;
    private float dir;
    private float goalHeight;

    void Start()
    {
        state = State.inactive;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        arenaX = transform.position.x;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

    void Update()
    {
        switch (state)
        {
            case State.inactive:
                if (transform.position.x - player.transform.position.x < 5f)
                {
                    rb.gravityScale = 2 * jumpHeight / Mathf.Pow(jumpTime / 2, 2) / 9.8f;
                    state = State.idle;
                    nextAttack = Time.time + 3f + attackCooldown;
                }
                break;
            case State.idle:
                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                if(Time.time > nextAttack)
                {
                    Jump();
                }
                break;
            case State.jumping:
                if(transform.position.y <= goalHeight && rb.velocity.y < 0)
                {
                    rb.velocity = Vector2.zero;
                    nextAttack = Time.time + attackCooldown;
                    state = State.idle;
                }
                break;
        }
    }

    private void Jump()
    {
        state = State.jumping;
        goalHeight = transform.position.y;
        goalPos = arenaX + Random.Range(-1, 2) * 11;
        dir = Mathf.Sign(goalPos - transform.position.x);
        transform.localScale = new Vector3(dir, 1, 1);
        float velocityX = (goalPos - transform.position.x) / jumpTime;
        float velocityY = rb.gravityScale * 4.9f * jumpTime;
        rb.velocity = new Vector2(velocityX, velocityY);
    }
}
