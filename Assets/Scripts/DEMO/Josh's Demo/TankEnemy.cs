using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    [SerializeField]
    private Collider2D dashHitbox;
    public override void Start()
    {
        base.Start();
        dashHitbox.enabled = false;
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
                case State.attacking:
                    dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                    break;
                case State.dashing:
                    float checkX1 = Mathf.Sign(rb.velocity.x) * 1f;
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
                    rb.velocity = new Vector2(1 * dir * dashSpeed, rb.velocity.y);
                    break;
            }
        }

        //stop at walls and cliffs

        float checkX = Mathf.Sign(rb.velocity.x) * 1f;
        bool isStopped = !Physics2D.OverlapCircle(transform.position + new Vector3(checkX, 0), .1f, tileMask) ||
                    Physics2D.OverlapCircle(transform.position + new Vector3(checkX, .5f), .3f, tileMask);
        if (isStopped)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            dir *= -1;
        }
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

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
        state = State.dashing;
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
    public override void DoAttack(RaycastHit2D hit)
    {
        anim.Play("dash");
        state = State.attacking;
    }
}
