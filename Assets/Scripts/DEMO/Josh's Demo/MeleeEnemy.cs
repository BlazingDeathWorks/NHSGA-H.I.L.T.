using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private Collider2D meleeHitbox;
    [SerializeField]
    private Collider2D dashHitbox;
    [SerializeField]
    private AudioClip meleeSound;

    public override void Start()
    {
        base.Start();
        meleeHitbox.enabled = false;
        dashHitbox.enabled = false;
        foreach (EnemyProjectileController hitbox in GetComponentsInChildren<EnemyProjectileController>())
        {
            hitbox.damage = damage;
        }
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), dashHitbox);
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            switch (state)
            {
                case State.idle:
                    rb.velocity = new Vector3(speed * dir, rb.velocity.y);
                    FindPlayer();
                    break;
                case State.dashing:
                    rb.velocity = new Vector2(1 * dir * dashSpeed, rb.velocity.y); 
                    float checkX1 = Mathf.Sign(rb.velocity.x) * .5f;
                    bool isStopped1 = !Physics2D.OverlapCircle(transform.position + new Vector3(checkX1, 0), .1f, tileMask) ||
                                Physics2D.OverlapCircle(transform.position + new Vector3(checkX1, .5f), .3f, tileMask);
                    if (isStopped1)
                    {
                        StopAttack();
                    }
                    break;
                case State.attacking:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }

        if (rb.velocity.x != 0)
        {
            //stop at walls and cliffs
            float checkX = Mathf.Sign(rb.velocity.x) * .5f;
            bool isStopped = !Physics2D.OverlapCircle(transform.position + new Vector3(checkX, 0), .1f, tileMask) ||
                        Physics2D.OverlapCircle(transform.position + new Vector3(checkX, .5f), .3f, tileMask);
            if (isStopped)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                dir *= -1;
            }
        }
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

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
        meleeHitbox.enabled = true;
        PlayMeleeSound();
    }
    private void DashAttack()
    {
        rb.velocity = new Vector2(1 * dir * dashSpeed, rb.velocity.y);
        state = State.dashing;
        dashHitbox.enabled = true;
        PlayDashSound();
    }

    private void StopAttack()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        meleeHitbox.enabled = false;
        dashHitbox.enabled = false;
        state = State.idle;
        nextAttack = Time.time + attackCooldown * Random.Range(.75f, 1.25f);
    }
    public override void DoAttack(RaycastHit2D hit)
    {
        if (hit.distance < meleeRange)
        {
            anim.Play("melee");
            state = State.attacking;
        }
        else
        {
            anim.Play("dash");
            state = State.attacking;
        }
    }
}
