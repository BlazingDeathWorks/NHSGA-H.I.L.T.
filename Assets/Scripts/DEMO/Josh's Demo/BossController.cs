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
    [SerializeField]
    private int staggerHits;
    [SerializeField]
    private GameObject sideWalls;
    [SerializeField]
    private Animator handHitbox;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private GameObject bombOrigin;
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private GameObject shotOrigin;
    [SerializeField]
    private float bombTime;

    public enum State
    {
        inactive, idle, jumping, attacking, bomb, defeated, staggered
    };

    private State state;
    private GameObject player;
    private float nextAttack;
    private Rigidbody2D rb;
    private float arenaX;
    private float goalPos;
    private float dir;
    private float goalHeight;
    private Animator anim;
    

    void Start()
    {
        //TODO CHANGE THIS
        Physics2D.IgnoreLayerCollision(8, 8);

        state = State.inactive;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
                    anim.enabled = true;
                    sideWalls.SetActive(true);
                    rb.gravityScale = 2 * jumpHeight / Mathf.Pow(jumpTime / 2, 2) / 9.8f;
                    state = State.idle;
                    nextAttack = Time.time + 1f + attackCooldown;
                }
                break;
            case State.idle:
                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                transform.localScale = new Vector3(dir, 1, 1);
                if (Time.time > nextAttack)
                {
                    float randAttack = Random.Range(0f, (Mathf.Abs(transform.position.x - arenaX) > 2f ? 1f : .75f));
                    if(randAttack <= .25f)
                    {
                        Jump();
                    } else if (randAttack <= .5f)
                    {
                        anim.Play("hand");
                        handHitbox.Play("handHitbox");
                        state = State.attacking;
                    }
                    else if (randAttack <= .75f)
                    {
                        anim.Play("throw");
                        state = State.bomb;
                    }
                    else
                    {
                        anim.Play("shootLasers");
                        state = State.attacking;
                    }
                }
                break;
            case State.jumping:
                if(transform.position.y <= goalHeight && rb.velocity.y < 0)
                {
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(transform.position.x, goalHeight);
                    SetIdle();
                }
                break;
            case State.defeated:
                break;
            case State.staggered:
                break;
        }
    }

    private void SetIdle()
    {
        nextAttack = Time.time + attackCooldown + (state == State.bomb ? bombTime : 0);
        state = State.idle;
        anim.SetTrigger("idle");
    }

    public void SpawnBomb()
    {
        //TODO create more bombs at lower health
        CreateBomb(); CreateBomb(); CreateBomb(); CreateBomb(); CreateBomb(); CreateBomb(); CreateBomb(); CreateBomb();
    }

    private void CreateBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombOrigin.transform.position, Quaternion.identity);
        Vector2 target = new Vector2(arenaX + Random.Range(-8f, 8f), transform.position.y + 5 + Random.Range(-3f, 7f));
        float velocityX = (target.x - bombOrigin.transform.position.x) / bombTime;
        float velocityY = ((target.y - bombOrigin.transform.position.y) + 4.9f * bombTime * bombTime) / bombTime;
        bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
        bomb.GetComponent<BombController>().explodeTime = Time.time + bombTime;
    }

    public void SpawnLasers()
    {
        float randOffset = Random.Range(-5f, 5f);
        for (int i = -30; i < 45; i += 15)
        {
            Instantiate(shotPrefab, shotOrigin.transform.position, Quaternion.Euler(0, 0, randOffset + i + (dir < 0 ? 180 : 0)));
        }
    }

    private void Jump()
    {
        anim.Play("jump");
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
