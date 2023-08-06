using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCard : MonoBehaviour
{
    public Vector2 ForceVector { get; set; }
    [HideInInspector] public NavigationButton Nb;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private bool launched;
    private bool oneFrame;

    private void Awake()
    {
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider.isTrigger = false;
    }

    private void FixedUpdate()
    { 
        if (!launched && !oneFrame)
        {
            rb.AddForce(ForceVector, ForceMode2D.Impulse);
            oneFrame = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (launched)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return;
            }
            boxCollider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Invincible");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }
        launched = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger");
            Nb?.UnlockButton();
            Destroy(gameObject);
        }
    }
}
