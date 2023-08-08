using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.1f;
    public float damageMod = 1;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(Weapon_ht.Instance.Damage * damageMod, gameObject);
            Ultimate_ht.Instance.InvokeActions(enemyHealth.gameObject);
        }
        if (collision.TryGetComponent(out ShopController shop) && shop.flashRenderer.color.a <= 0)
        {
            shop.OnHit();
        }
    }
}
