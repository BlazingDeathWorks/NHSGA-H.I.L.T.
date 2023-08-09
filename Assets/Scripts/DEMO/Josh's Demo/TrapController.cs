using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField]
    private float launchScale;
    [SerializeField]
    private float damage;

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer != LayerMask.NameToLayer("Invincible"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-Mathf.Sign(rb.velocity.x), 1) * launchScale;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
