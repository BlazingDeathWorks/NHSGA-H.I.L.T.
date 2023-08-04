using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float aggroDistance;
    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private LayerMask tileMask;
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private GameObject meleeHitbox;
    [SerializeField]
    private GameObject dashHitbox;
    [SerializeField]
    private AudioClip warningSound;
    [SerializeField]
    private AudioClip meleeSound;
    [SerializeField]
    private AudioClip dashSound;

    private GameObject player;
    private AudioManager audioManager;
    private Animator anim;
    private Rigidbody2D rb;
    private bool active;
    private float dir;
    private State state;
    private float nextAttack;
    private float aggroTime;

    public enum State
    {
        idle, attacking
    };
    void Start()
    {
        player = GameObject.Find("Player");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        active = false;
        dir = Random.Range(0, 2) * 2 - 1;
    }

    void Update()
    {
        if (!active)
        {
            active = (player.transform.position - transform.position).magnitude < 25f;
        }
        if (active)
        {
            switch (state)
            {
                case State.idle:
                    rb.velocity = new Vector3(speed * dir, rb.velocity.y);
                    FindPlayer();
                    break;
            }
        }

        //stop at walls and cliffs
        float checkX = Mathf.Sign(rb.velocity.x) * 1.5f;
        bool isStopped = !Physics2D.OverlapCircle(transform.position + new Vector3(checkX, 0), .1f, tileMask) ||
                    Physics2D.OverlapCircle(transform.position + new Vector3(checkX, .5f), .3f, tileMask);
        if (isStopped)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            dir *= -1;
        }
        transform.localScale = new Vector3(dir, 1, 1);
    }
    public void SetStats(float scaler)
    {
        scaler = 1.2f - scaler;
        damage *= scaler;
        GetComponent<EnemyHealth>().MultiplyHealth(scaler);
        meleeHitbox.GetComponent<EnemyProjectileController>().damage = damage;
        dashHitbox.GetComponent<EnemyProjectileController>().damage = damage;
    }

    public void SetElite()
    {
        damage *= 1.5f;
        speed *= 1.5f;
        GetComponent<EnemyHealth>().MultiplyHealth(2f);
        GetComponent<SpriteRenderer>().color = new Color(1, .2f, .2f);
    }
    private void PlayWarningSound()
    {
        audioManager.PlayOneShot(warningSound);
    }
    private void PlayMeleeSound()
    {
        audioManager.PlayOneShot(meleeSound);
    }
    private void PlayDashSound()
    {
        audioManager.PlayOneShot(dashSound);
    }

    private void MeleeAttack()
    {
        meleeHitbox.SetActive(true);
        PlayMeleeSound();
    }
    private void DashAttack()
    {
        dashHitbox.SetActive(true);
        PlayMeleeSound();
    }

    private void StopAttack()
    {
        meleeHitbox.SetActive(false);
        dashHitbox.SetActive(false);
        state = State.idle;
    }

    private void FindPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right * dir, aggroDistance, playerMask);
        bool canSeePlayer = hit.collider != null;
        if (!canSeePlayer)
        {
            hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.left * dir, 2f, playerMask);
            canSeePlayer = hit.collider != null;
        }
        if (canSeePlayer && Time.time > nextAttack)
        {
            if(aggroTime < Time.time)
            {
                state = State.attacking;
                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                if (hit.distance < meleeRange)
                {
                    anim.Play("Melee");
                } else
                {
                    anim.Play("Dash");
                }
                nextAttack = Time.time + attackCooldown * Random.Range(.75f, 1.25f);
            }
        } else
        {
            aggroTime = Time.time + .5f;
        }

    }
}
