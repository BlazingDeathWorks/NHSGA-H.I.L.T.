using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDamager : MonoBehaviour
{
    private PlayerEntity playerEntity;
    private float rollDamage = 0;

    private void Awake()
    {
        playerEntity = GetComponentInParent<PlayerEntity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            //Check if we are sliding rn
            if (playerEntity.CanSlide) return;
            if (rollDamage == 0) return;
            enemyHealth.TakeDamage(rollDamage);
        }
    }

    public void SetRollDamage(float rollDamage)
    {
        this.rollDamage = rollDamage;
    }
}
