using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float damage;
    public float speed;
    
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
        Destroy(gameObject);
    }
}
