using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int aiType;

    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask tileMask;

    private GameObject player;
    private Rigidbody2D rb;
    private bool active;
    private float dir;
    private float leftBound;
    private float rightBound;
    private State state;

    public enum State
    {
        idle, aggro, attacking
    };
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        active = false;
        dir = Random.Range(0, 2) * 2 - 1;
        leftBound = rightBound = -100;
    }

    void Update()
    {
        if ((player.transform.position - transform.position).magnitude < 50f)
        {
            active = true;
        }
        if (active)
        {
            switch (state)
            {
                case State.idle:
                    Patrol();
                    //Ray canSeePlayer = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right * dir);
                    break;
                case State.aggro:
                    switch (aiType)
                    {
                        case 0:

                            break;
                    }
                    break;
            }
        }
    }

    private void Patrol()
    {
        rb.velocity = Vector3.right * speed * dir;
        //set patrol bounds
        if (leftBound == -100 && dir < 0)
        {
            bool isStopped = !Physics2D.OverlapCircle(transform.position + Vector3.left, .1f, tileMask) ||
                Physics2D.OverlapPoint(transform.position + new Vector3(-1, .5f), tileMask);
            if (isStopped)
            {
                leftBound = transform.position.x;
            }
        }
        if (rightBound == -100 && dir > 0)
        {
            bool isStopped = !Physics2D.OverlapCircle(transform.position + Vector3.right, .1f, tileMask) ||
                Physics2D.OverlapPoint(transform.position + new Vector3(1, .5f), tileMask);
            if (isStopped)
            {
                rightBound = transform.position.x;
            }
        }

        //flip at bounds
        if (leftBound != -100 && transform.position.x < leftBound) dir = 1;
        if (rightBound != -100 && transform.position.x > rightBound) dir = -1;
    }
}
