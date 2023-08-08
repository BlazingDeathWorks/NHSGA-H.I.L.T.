using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer flashRenderer;
    [SerializeField]
    private float startAttackCooldown;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private float jumpHeight;
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
    [SerializeField]
    private GameObject healthbar;
    [SerializeField]
    private float staggerTime;
    [SerializeField]
    private int hitsToStagger;
    [SerializeField]
    private float passiveAttackSpeed;
    [SerializeField]
    private float passiveAttackRange;
    [SerializeField]
    private GameObject passiveAttack;
    [SerializeField]
    private AudioClip warningSound;
    [SerializeField]
    private AudioClip spawnSound;
    [SerializeField]
    private AudioClip meleeSound;
    [SerializeField]
    private AudioClip landSound;
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip throwSound;
    [SerializeField]
    private AudioClip laserSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip explosionSound;
    [SerializeField]
    private AudioClip bossSong;

    public enum State
    {
        inactive, idle, jumping, attacking, bomb, defeated, staggered
    };

    private State state;
    private GameObject player;
    private float nextAttack;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float arenaX;
    private float arenaY;
    private float goalPosX;
    private float dir;
    private float goalPosY;
    private Animator anim;
    private float staggerTimer;
    private int[] attackCounts;
    private float attackCooldown;
    private EnemyHealth healthScript;
    private int staggerHitCount;
    private AudioManager audioManager;
    private Collider2D handHitboxCollider;
    private bool activated;

    void Start()
    {

        state = State.inactive;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        healthScript = GetComponent<EnemyHealth>();
        handHitboxCollider = handHitbox.GetComponent<Collider2D>();
        arenaX = transform.position.x;
        arenaY = transform.position.y;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        attackCounts = new int[3];
    }

    void Update()
    {
        attackCooldown = startAttackCooldown * (.5f + healthScript.GetPortionHealth() / 2f);

        //normal attack patterns
        switch (state)
        {
            case State.inactive:
                CheckActivate();
                break;
            case State.idle:
                Idle();
                break;
            case State.jumping:
                CheckJump();
                break;
            case State.defeated:
                break;
            case State.staggered:
                DoStagger();
                break;
        }

        PassiveAttack();

        flashRenderer.sprite = sprite.sprite;
        if (flashRenderer.color.a > 0) flashRenderer.color = new Color(.75f, .75f, .75f, flashRenderer.color.a - 2 * Time.deltaTime);

    }

    private void PassiveAttack()
    {
        if (healthScript.GetPortionHealth() < .5f)
        {
            passiveAttack.transform.position += Vector3.right * passiveAttackSpeed * Time.deltaTime;
            if (Mathf.Abs(passiveAttack.transform.position.x) > passiveAttackRange)
            {
                int newDir = 1 - Random.Range(0, 2) * 2;
                passiveAttack.transform.position = new Vector3((passiveAttackRange-.5f) * newDir * -1,
                    passiveAttack.transform.position.y);
                passiveAttackSpeed = Mathf.Abs(passiveAttackSpeed) * newDir;
            }
        }
    }

    private void DoStagger()
    {
        staggerTimer += Time.deltaTime;
        if (staggerTimer > staggerTime)
        {
            staggerTimer = 0;
            SetIdle();
            staggerHitCount = 0;
        }
    }

    private void CheckJump()
    {
        if (transform.position.y <= goalPosY && rb.velocity.y < 0)
        {
            audioManager.PlayOneShot(landSound);
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, goalPosY);
            Instantiate(shotPrefab, transform.position + new Vector3(0, .5f), Quaternion.Euler(0, 0, 0));
            Instantiate(shotPrefab, transform.position + new Vector3(0, .5f), Quaternion.Euler(0, 0, 180));
            SetIdle();
        }
    }

    private void Idle()
    {
        dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        transform.localScale = new Vector3(dir, 1, 1);
        if (Time.time > nextAttack)
        {
            //randomize smartly
            int minAttack = attackCounts[0];
            foreach (int x in attackCounts)
            {
                minAttack = Mathf.Min(minAttack, x);
            }
            int randAttack = Random.Range(0, 3);
            while (attackCounts[randAttack] > minAttack + 1)
            {
                randAttack = Random.Range(0, 3);
            }
            attackCounts[randAttack]++;
            if (healthScript.GetPortionHealth() < .1f && randAttack == 0) randAttack = Random.Range(1, 3);

            //perform attack
            switch (randAttack)
            {
                case 0:
                    Jump();
                    break;
                case 1:
                    PlayWarningSound();
                    if (Mathf.Abs(transform.position.y - arenaY) < 1f)
                    {
                        anim.Play("hand");
                        handHitbox.Play("handHitbox");
                    }
                    else
                    {
                        anim.Play("shootLasers");
                    }
                    state = State.attacking;
                    break;
                case 2:
                    anim.Play("throw");
                    state = State.bomb;
                    break;
            }
        }
    }

    public void AddStagger()
    {
        staggerHitCount++;
        if(staggerHitCount > hitsToStagger && state != State.staggered && state != State.jumping && Mathf.Abs(transform.position.y - arenaY) < 1f){
            anim.Play("stagger");
            handHitboxCollider.enabled = false;
            handHitbox.Play("start");
            state = State.staggered;
        }
    }

    private void CheckActivate()
    {
        if (arenaX - player.transform.position.x < 5f && !activated)
        {
            sideWalls.SetActive(true);
            audioManager.PlaySong(bossSong);
            activated = true;
            Invoke("Activate", 1f);
        }
    }

    private void Activate()
    {
        audioManager.PlayOneShot(spawnSound);
        anim.enabled = true;
        rb.gravityScale = 2 * jumpHeight / Mathf.Pow(jumpTime / 2, 2) / 9.8f;
        state = State.staggered;
        nextAttack = Time.time + 1f + attackCooldown;
        healthbar.SetActive(true);
    }

    private void SetIdle()
    {
        nextAttack = Time.time + attackCooldown + (state == State.bomb ? bombTime : 0);
        state = State.idle;
        anim.SetTrigger("idle");
    }

    public void SpawnBomb()
    {
        audioManager.PlayOneShot(throwSound);
        Invoke("PlayExplosionSound", bombTime);
        int bombCount = 5 - Mathf.FloorToInt(healthScript.GetPortionHealth()*3) * 2;
        for (int i = 0; i < bombCount; i++)
        {
            CreateBomb();
        }
    }

    private void CreateBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombOrigin.transform.position, Quaternion.identity);
        Vector2 target = new Vector2(arenaX + Random.Range(-8f, 8f), Random.Range(2f, 8f));
        float velocityX = (target.x - bombOrigin.transform.position.x) / bombTime;
        float velocityY = ((target.y - bombOrigin.transform.position.y) + 4.9f * bombTime * bombTime) / bombTime;
        bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
        bomb.GetComponent<BombController>().explodeTime = Time.time + bombTime;
    }
    private void PlayExplosionSound()
    {
        audioManager.PlayOneShot(explosionSound);
    }

    public void SpawnLasers()
    {
        audioManager.PlayOneShot(laserSound);
        float randOffset = Random.Range(-5f, 5f);
        for (int i = -90; i < 0; i += 10)
        {
            Instantiate(shotPrefab, shotOrigin.transform.position, Quaternion.Euler(0, 0, randOffset + (dir < 0 ? 180 - i: i)));
        }
    }
    public SpriteRenderer GetFlashRenderer()
    {
        return flashRenderer;
    }
    public void Die()
    {
        audioManager.PlayOneShot(deathSound);
        anim.Play("death");
        passiveAttack.SetActive(false);
        sideWalls.SetActive(false);
        state = State.defeated;
    }

    private void PlayWarningSound()
    {
        audioManager.PlayOneShot(warningSound);
    }

    public void DeleteObject()
    {
        Destroy(gameObject);
    }
    private void Jump()
    {
        audioManager.PlayOneShot(jumpSound);
        anim.Play("jump");
        state = State.jumping;
        goalPosY = transform.position.y - (Mathf.Abs(transform.position.y - arenaY) > 1f ? 6f : 0);
        goalPosX = arenaX + (Mathf.Abs(transform.position.y - arenaY) < 1f ? (1 + Random.Range(-1, 1) * 2) * 11: 0);
        if(goalPosX != arenaX) goalPosY += 6f;
        dir = Mathf.Sign(goalPosX - transform.position.x);
        transform.localScale = new Vector3(dir, 1, 1);
        float velocityX = (goalPosX - transform.position.x) / jumpTime;
        float velocityY = (goalPosY - transform.position.y) / jumpTime + rb.gravityScale * 4.9f * jumpTime;
        rb.velocity = new Vector2(velocityX, velocityY);
    }
}
