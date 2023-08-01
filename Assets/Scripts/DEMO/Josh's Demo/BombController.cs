using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float explodeTime;

    [SerializeField]
    private GameObject shotPrefab;

    private Animator anim;
    private Rigidbody2D rb;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(Time.time > explodeTime)
        {
            anim.Play("explode");
            rb.velocity = Vector2.zero;
        }
    }

    public void SpawnLasers()
    {
        for (int i=0; i< 360; i+= 45)
        {
            Instantiate(shotPrefab, transform.position, Quaternion.Euler(0, 0, i));
        }
        Destroy(gameObject);
    }
}
