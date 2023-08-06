using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer flashRenderer;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float dashSpeed;
    [SerializeField]
    protected float attackCooldown;
    [SerializeField]
    protected float aggroDistance;
    [SerializeField]
    protected LayerMask tileMask;
    [SerializeField]
    protected LayerMask visionMask;
    [SerializeField]
    protected AudioClip warningSound;
    [SerializeField]
    protected AudioClip dashSound;

    protected GameObject player;
    protected Collider2D playerCollider;
    protected AudioManager audioManager;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected float dir;
    protected State state;
    protected float nextAttack;
    protected SpriteRenderer sprite;
    protected float aggroTime;

    protected bool active;

    public enum State
    {
        idle, attacking, dead
    };

    public virtual void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        active = false;
        dir = Random.Range(0, 2) * 2 - 1;
    }

    public virtual void Update()
    {
        if (!active)
        {
            active = (player.transform.position - transform.position).magnitude < 25f;
        }
        flashRenderer.sprite = sprite.sprite;
        if(flashRenderer.color.a > 0) flashRenderer.color = new Color(.75f, .75f, .75f, flashRenderer.color.a - 2 * Time.deltaTime);
    }
    public virtual void DoAttack(RaycastHit2D hit) { }
    protected void FindPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right * dir, aggroDistance, visionMask);
        bool canSeePlayer = hit.collider != null && hit.collider.Equals(playerCollider);
        if (!canSeePlayer)
        {
            hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.left * dir, 3f, visionMask);
            canSeePlayer = hit.collider != null && hit.collider.Equals(playerCollider);
        }
        if (canSeePlayer && Time.time > nextAttack)
        {
            if (aggroTime < Time.time)
            {
                state = State.attacking;
                rb.velocity = new Vector2(0, rb.velocity.y);
                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                DoAttack(hit);
            }
        }
        else
        {
            aggroTime = Time.time + .2f;
        }
    }
    public void SetStats(float scaler)
    {
        scaler = 1.2f - scaler;
        damage *= scaler;
        GetComponent<EnemyHealth>().MultiplyHealth(scaler);
    }

    public void SetElite()
    {
        damage *= 1.5f;
        speed *= 1.5f;
        GetComponent<EnemyHealth>().MultiplyHealth(2f);
        GetComponent<SpriteRenderer>().color = new Color(1, .2f, .2f);
    }

    public SpriteRenderer GetFlashRenderer()
    {
        return flashRenderer;
    }

    public void Die()
    {
        anim.Play("death");
        state = State.dead;
        enabled = false;
    }

    public void DeleteObject()
    {
        Destroy(gameObject);
    }
}
