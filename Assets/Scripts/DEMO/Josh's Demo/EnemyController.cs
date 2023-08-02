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
    private float damage;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private LayerMask tileMask;
    [SerializeField]
    private EnemyProjectileController projectilePrefab;
    [SerializeField]
    private GameObject projectileOrigin;
    [SerializeField]
    private float aggroFollowDistance;
    [SerializeField]
    private float aggroDistance;
    [SerializeField]
    private AudioClip aggroSound;
    [SerializeField]
    private AudioClip attackSound;

    private GameObject player;
    private AudioManager audioManager;
    private Collider2D playerCollider;
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D rb;
    private bool active;
    private float dir;
    private float leftBound;
    private float rightBound;
    private State state;
    private float nextAttack;
    private float aggroGoalX;
    private bool attackDelayed;
    private Collider2D hitboxCollider;

    public enum State
    {
        idle, aggro, attacking
    };
    void Start()
    {
        player = GameObject.Find("Player");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        active = false;
        dir = Random.Range(0, 2) * 2 - 1;
        rightBound = leftBound = -100;
        Invoke("SetPatrolBounds", 0.01f);
        if (aiType == 1 || aiType == 2)
        {
            projectilePrefab.GetComponent<Collider2D>().enabled = false;
            projectilePrefab.damage = damage;
            hitboxCollider = projectilePrefab.GetComponent<Collider2D>();
        }
    }

    private void SetPatrolBounds()
    {
        float posCheck = 1.5f;
        while (leftBound == -100 || rightBound == -100)
        { 
            if (leftBound == -100)
            {
                bool isEdge = !Physics2D.OverlapCircle(transform.position + new Vector3(-posCheck, 0), 0.1f, tileMask);
                bool isWall = Physics2D.OverlapPoint(transform.position + new Vector3(-posCheck, .5f), tileMask);
                if (isEdge || isWall)
                {
                    leftBound = transform.position.x - (posCheck - 1);
                }
            }
            if (rightBound == -100)
            {
                bool isStopped = !Physics2D.OverlapCircle(transform.position + new Vector3(posCheck, 0), .1f, tileMask) ||
                    Physics2D.OverlapCircle(transform.position + new Vector3(posCheck, .5f), .1f, tileMask);
                if (isStopped)
                {
                    rightBound = transform.position.x + (posCheck - 1);
                }
            }
            posCheck += 1;
            if (posCheck > 100) break;
        }
    }

    void Update()
    {
        if (!active && (player.transform.position - transform.position).magnitude < 25f)
        {
            active = true;
        }
        if (active)
        {
            switch (state)
            {
                case State.idle:
                    Patrol();
                    if (FindPlayer(true) && aiType == 2)
                    {
                        PlayAggroSound();
                    }
                    break;

                case State.aggro:
                    switch (aiType)
                    {
                        case 0:
                            if(MoveToAggro(1.5f)) dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                            if (Time.time > nextAttack)
                            {
                                state = State.attacking;
                                anim.SetTrigger("attack");
                            }
                            break;
                        case 1:
                            if (MoveToAggro(2f)) nextAttack = Time.time - 1f;
                            aggroGoalX = Mathf.Clamp(player.transform.position.x +
                                aggroFollowDistance * (transform.position.x < player.transform.position.x ? -1 : 1), leftBound, rightBound);
                            if (Time.time > nextAttack)
                            {
                                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                                state = State.attacking;
                                anim.SetTrigger("attack");
                                rb.velocity = Vector2.zero;
                            }
                            break;
                        case 2:
                            rb.velocity = Vector2.zero;
                            dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                            if (!FindPlayer(false) && !attackDelayed)
                            {
                                nextAttack = Time.time + .75f;
                                attackDelayed = true;
                            }
                            if (Time.time > nextAttack)
                            {
                                rb.velocity = Vector2.right * dir * speed * 5f;
                                state = State.attacking;
                                anim.SetTrigger("attack");
                                SpawnHitbox();
                            }
                            break;
                    }
                    break;

                case State.attacking:
                    switch (aiType)
                    {
                        case 2:
                            hitboxCollider.enabled = true;
                            float thisX = transform.position.x;
                            if(dir < 0)
                            {
                                if(thisX - player.transform.position.x < -2 || thisX < leftBound)
                                {
                                    anim.SetTrigger("idle");
                                    DespawnHitbox();
                                    state = State.idle;
                                }
                            } else
                            {
                                if (thisX - player.transform.position.x > 2 || thisX > rightBound)
                                {
                                    anim.SetTrigger("idle");
                                    DespawnHitbox();
                                    state = State.idle;
                                }
                            }
                            break;
                    }
                    break;
            }
        }
        projectileOrigin.transform.localPosition = new Vector3(Mathf.Abs(projectileOrigin.transform.localPosition.x) *
            (dir < 0 ? -1 : 1), projectileOrigin.transform.localPosition.y);
        sprite.flipX = dir < 0;
    }

    private bool MoveToAggro(float speedMod)
    {
        float thisX = transform.position.x;
        float playerX = player.transform.position.x;
        if (Mathf.Abs(aggroGoalX - thisX) > .2f)
        {
            dir = Mathf.Sign(aggroGoalX - thisX);
            rb.velocity = Vector2.right * speed * dir * speedMod;
            return false;
        }
        else
        {
            rb.velocity = Vector2.zero;
            dir = Mathf.Sign(playerX - thisX);
            return true;
        }
    }

    public void SetStats(float scaler)
    {
        scaler = 1.2f - scaler;
        damage *= scaler;
        speed *= scaler * Random.Range(.9f, 1.1f);
        GetComponent<EnemyHealth>().MultiplyHealth(scaler);
    }

    public void SetElite()
    {
        damage *= 1.5f;
        speed *= 1.5f;
        GetComponent<EnemyHealth>().MultiplyHealth(2f);
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    private void PlayAggroSound()
    {
        audioManager.PlayOneShot(aggroSound);
    }
    private void PlayAttackSound()
    {
        audioManager.PlayOneShot(attackSound);
    }
    private void AttackRanged()
    {
        Quaternion angle = dir > 0 ? Quaternion.Euler(0, 0, 42) : Quaternion.Euler(0, 0, 222);
        EnemyProjectileController projectile = Instantiate(projectilePrefab, projectileOrigin.transform.position, angle);
        projectile.damage = damage;
        state = State.idle;
    }

    private void SpawnHitbox()
    {
        projectilePrefab.GetComponent<Collider2D>().enabled = true;
        PlayAttackSound();
        Physics2D.IgnoreCollision(playerCollider, projectilePrefab.GetComponent<Collider2D>(), false);
    }

    private void DespawnHitbox()
    {
        projectilePrefab.GetComponent<Collider2D>().enabled = false;
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
        state = State.idle;
    }

    private bool FindPlayer(bool setAttack)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right * dir, aggroDistance);
        bool canSeePlayer = hit.collider != null && hit.collider.Equals(playerCollider);
        if (!canSeePlayer)
        {
            hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.left * dir, 2f);
            canSeePlayer = hit.collider != null && hit.collider.Equals(playerCollider);
        }
        if (canSeePlayer && setAttack)
        {
            aggroGoalX = Mathf.Clamp(player.transform.position.x +
                aggroFollowDistance * (transform.position.x < player.transform.position.x ? -1 : 1), leftBound, rightBound);
            nextAttack = Time.time + attackCooldown * Random.Range(.75f, 1.25f);
            anim.SetTrigger("aggro");
            state = State.aggro;
            attackDelayed = false;
        }
        return canSeePlayer;

    }

    private void Patrol()
    {
        rb.velocity = Vector3.right * speed * dir;

        //flip at bounds
        if (transform.position.x < leftBound) dir = 1;
        if (transform.position.x > rightBound) dir = -1;
    }
}
