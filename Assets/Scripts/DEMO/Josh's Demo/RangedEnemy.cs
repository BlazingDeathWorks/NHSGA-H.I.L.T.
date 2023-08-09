using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
   
    [SerializeField]
    private float dashRange;
    [SerializeField]
    private EnemyProjectileController laserPrefab;
    [SerializeField]
    private GameObject projectileOrigin;
    [SerializeField]
    private AudioClip shootSound;
   
    public override void Update()
    {
        base.Update();
        if (active)
        {
            switch (state)
            {
                case State.idle:
                    rb.velocity = new Vector2(speed * dir, rb.velocity.y);
                    FindPlayer();
                    break;
                case State.dashing:
                    rb.velocity = new Vector2(1 * Mathf.Sign(transform.position.x - player.transform.position.x) * dashSpeed, rb.velocity.y);
                    float checkX1 = Mathf.Sign(rb.velocity.x) * .5f;
                    bool isStopped1 = !Physics2D.OverlapCircle(transform.position + new Vector3(checkX1, 0), .1f, tileMask) ||
                                Physics2D.OverlapCircle(transform.position + new Vector3(checkX1, .5f), .3f, tileMask);
                    if (isStopped1)
                    {
                        anim.Play("attack");
                        state = State.attacking;
                    }
                    dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

                    break;
                case State.attacking:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }

        if(state == State.idle)
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
    private void PlayShootSound()
    {
        audioManager.PlayOneShot(shootSound);
    }
    private void PlayDashSound()
    {
        audioManager.PlayOneShot(dashSound);
    }

    private void RangedAttack()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

        Quaternion angle = dir > 0 ? Quaternion.Euler(0, 0, 42) : Quaternion.Euler(0, 0, 222);
        EnemyProjectileController projectile = Instantiate(laserPrefab, projectileOrigin.transform.position, angle);
        projectile.damage = damage;
        PlayShootSound();
        nextAttack = Time.time + attackCooldown * Random.Range(.75f, 1.25f);
        state = State.idle;
        anim.Play("rangedIdle");
    }
    private void DashAttack()
    {
        rb.velocity = new Vector2(1 * Mathf.Sign(transform.position.x - player.transform.position.x) * dashSpeed, rb.velocity.y);
        state = State.dashing;
        PlayDashSound();
    }

    private void StopDash()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        dir = Mathf.Sign(player.transform.position.x - transform.position.x);
    }

    public override void DoAttack(RaycastHit2D hit)
    {
        if (hit.distance < dashRange)
        {
            anim.Play("dash");
            state = State.attacking;
        }
        else
        {
            anim.Play("attack");
            state = State.attacking;
        }
    }
}
