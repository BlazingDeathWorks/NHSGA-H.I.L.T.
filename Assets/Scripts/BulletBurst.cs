using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBurst : MonoBehaviour
{
    [SerializeField] private Transform slashSpawnPos;
    [SerializeField] private PlayerProjectile projectilePrefab;
    [SerializeField] private Vector2 offset;
    private int bulletBurst = 0;

    public void SpawnBurst()
    {
        for (int i = 0; i < 15 * bulletBurst; i += 15)
        {
            Instantiate(projectilePrefab, slashSpawnPos.position, Quaternion.Euler(0, 0, i * Mathf.Sign(transform.localScale.x) + (transform.localScale.x < 0 ? 180 : 0)));
        }
    }

    public void SetBulletBurst(int bulletBurst)
    {
        this.bulletBurst = bulletBurst;
    }
}
