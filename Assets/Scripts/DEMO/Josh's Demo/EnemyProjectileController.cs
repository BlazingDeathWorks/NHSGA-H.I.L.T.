using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool noDestroy;
    
    private Rigidbody2D rb;
    private GameObject cam;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Mathf.Sign(transform.right.x) * Vector2.right * speed;
        cam = Camera.main.gameObject;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
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
