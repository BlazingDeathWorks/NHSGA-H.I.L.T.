using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float damage;

    [SerializeField]
    private float speed;
    [SerializeField]
    private bool noDestroy;
    [SerializeField]
    private bool offsetRot;
    
    private Rigidbody2D rb;
    private Camera cam;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = offsetRot ? Mathf.Sign(transform.right.x) * Vector2.right * speed : transform.right * speed;
        cam = Camera.main;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invincible") || collision.TryGetComponent(out RollDamager roll)) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health.GetComponent<PlayerEntity>().enabled = false;
            health.ForceVector = new Vector2(Mathf.Sign(collision.transform.position.x - transform.position.x) * 87.5f, 5);
            health.TakeDamage(damage);
        }
        if (noDestroy)
        {
            GetComponent<Collider2D>().enabled = false;
        } else
        {
            Destroy(gameObject);
        }
    }
}
