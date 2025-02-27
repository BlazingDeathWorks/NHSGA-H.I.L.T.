using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon_ht : MonoBehaviour
{
    public static Weapon_ht Instance { get; private set; }

    //STEP #1 - Create a variable that its value will be referenced in another script
    public int Damage { get; private set; } = 10;
    public float Knockback { get; private set; } = 0;
    public float StunTime { get; private set; } = 0;
    public float PoisonDamage { get; private set; } = 0;
    public float PoisonRate { get; private set; } = 5;

    //STEP #2 (Maybe) - Make event for Enemy Health type stuff
    public event Action<float> OnSetKnockback;
    public event Action<float> OnSetStunTime;
    public event Action<float> OnSetPoisonDamage;
    public event Action<float> OnSetPoisonRate;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    //STEP #3 - Create a setter method that'll be called in the onCodeBlockEnabled() of the corresponding PNB
    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    public void SetKnockback(float knockback)
    {
        Knockback = knockback;
        OnSetKnockback?.Invoke(knockback);
    }

    public void SetStunTime(float stunTime)
    {
        StunTime = stunTime;
        OnSetStunTime?.Invoke(stunTime);
    }

    public void SetPoisonDamage(float damage)
    {
        PoisonDamage = damage;
        OnSetPoisonDamage?.Invoke(damage);
    }

    //How long it lasts
    public void SetPoisonRate(float rate)
    {
        PoisonRate = rate;
        OnSetPoisonRate?.Invoke(rate);
    }
}
