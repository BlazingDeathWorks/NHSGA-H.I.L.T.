using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ht : MonoBehaviour
{
    public static Weapon_ht Instance { get; private set; }

    //STEP #1 - Create a variable that its value will be referenced in another script
    public int Damage { get; private set; } = 1;
    public float Knockback { get; private set; } = 0;
    public float StunTime { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    //STEP #2 - Create a setter method that'll be called in the onCodeBlockEnabled() of the corresponding PNB
    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    public void SetKnockback(float knockback)
    {
        Knockback = knockback;
    }

    public void SetStunTime(float stunTime)
    {
        StunTime = stunTime;
    }
}
