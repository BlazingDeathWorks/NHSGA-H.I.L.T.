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
            if(aggroTime < Time.time)
            {
                state = State.attacking;
                rb.velocity = new Vector2(0, rb.velocity.y);
                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                if (hit.distance < meleeRange)
                {
                    anim.Play("melee");
                } else
                {
                    anim.Play("dash");
                }
            }
        } else
        {
            aggroTime = Time.time + .3f;
        }

    }
}
