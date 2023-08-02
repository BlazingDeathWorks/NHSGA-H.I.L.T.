using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(Weapon_ht.Instance.Damage);
            Debug.Log("DAMAGE ENEMY");
            //Call stuff like enemyHealth.Stun() and so on for each effect
        }
    }
}
