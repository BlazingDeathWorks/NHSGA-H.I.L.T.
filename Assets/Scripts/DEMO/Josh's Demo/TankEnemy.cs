using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    [SerializeField]
    private Collider2D dashHitbox;

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
                case State.attacking:
                    if(rb.velocity.magnitude < .2f) dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                    float checkX1 = Mathf.Sign(rb.velocity.x) * 1.5f;
                    bool isStopped1 = !Physics2D.OverlapCircle(transform.position + new Vector3(checkX1, 0), .1f, tileMask) ||
                                Physics2D.OverlapCircle(transform.position + new Vector3(checkX1, .5f), .3f, tileMask);
                    if (isStopped1) StopAttack();
                    if (dir < 0)
                    {
                        if (transform.position.x - player.transform.position.x < -2) StopAttack();
                    }
                    else
                    {
                        if (transform.position.x - player.transform.position.x > 2) StopAttack();
                    }
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
    private void PlayDashSound()
    {
        audioManager.PlayOneShot(dashSound);
    }
    private void DashAttack()
    {
        rb.velocity = new Vector2(1 * dir * dashSpeed, rb.velocity.y);
        dashHitbox.enabled = true;
        PlayDashSound();
    }

    private void StopAttack()
    {
        anim.Play("idle");
        rb.velocity = new Vector2(0, rb.velocity.y);
        dashHitbox.enabled = false;
        state = State.idle;
        nextAttack = Time.time + attackCooldown * Random.Range(.75f, 1.25f);
    }
    private bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right * dir, aggroDistance, visionMask);
        bool canSeePlayer = hit.collider != null && hit.collider.Equals(playerCollider);
        if (!canSeePlayer)
        {
            hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.left * dir, 3f, visionMask);
            canSeePlayer = hit.collider != null && hit.collider.Equals(playerCollider);
        }
        return canSeePlayer;
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
                anim.Play("dash");
            }
        }
        else
        {
            aggroTime = Time.time + .3f;
        }
    }
}
