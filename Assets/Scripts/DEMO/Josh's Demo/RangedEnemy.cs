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
            }
        }

        rb.AddForce(Vector2.up * .1f, ForceMode2D.Force);

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

        flashRenderer.sprite = sprite.sprite;
        flashRenderer.color -= new Color(0, 0, 0, 2f * Time.deltaTime);
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
        Quaternion angle = dir > 0 ? Quaternion.Euler(0, 0, 42) : Quaternion.Euler(0, 0, 222);
        EnemyProjectileController projectile = Instantiate(laserPrefab, projectileOrigin.transform.position, angle);
        projectile.damage = damage;
        PlayShootSound();
        nextAttack = Time.time + attackCooldown * Random.Range(.75f, 1.25f);
        state = State.idle;
    }
    private void DashAttack()
    {
        rb.velocity = new Vector2(1 * Mathf.Sign(transform.position.x - player.transform.position.x) * dashSpeed, rb.velocity.y);
        PlayDashSound();
    }

    private void StopDash()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        dir = Mathf.Sign(player.transform.position.x - transform.position.x);
    }

    private void FindPlayer()
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
                if (hit.distance < dashRange)
                {
                    anim.Play("dash");
                }
                else
                {
                    anim.Play("attack");
                }
            }
        }
        else
        {
            aggroTime = Time.time + .3f;
        }

    }

}
