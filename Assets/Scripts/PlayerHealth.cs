using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private Slider healthbar;
    [SerializeField]
    private float healthbarSpeed;

    private float immuneTime;
    private bool immune;

    //Default - 0
    private float healAmount = 10;
    //Default - 15
    private float healRate = 5;
    private float timeSinceLastHeal;

    void Start()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = health;
        healthbar.value = health;
    }
    void Update()
    {
        timeSinceLastHeal += Time.deltaTime;
        if (timeSinceLastHeal >= healRate)
        {
            timeSinceLastHeal = 0;
            health += healAmount;
        }
        health = Mathf.Clamp(health, 0, healthbar.maxValue);

        float healthbarVal = healthbar.value;
        if (health > healthbarVal)
        {
            healthbarVal += Mathf.CeilToInt((health - healthbarVal) / healthbarSpeed) * healthbarSpeed * Time.deltaTime;
            if (healthbarVal > health) healthbarVal = health;
        }
        if (health < healthbarVal)
        {
            healthbarVal -= Mathf.CeilToInt((healthbarVal - health) / healthbarSpeed) * healthbarSpeed * Time.deltaTime;
            if (healthbarVal < health) healthbarVal = health;
        }
        healthbar.value = healthbarVal;

        if(Time.time < immuneTime && !immune)
        {
            gameObject.layer = LayerMask.NameToLayer("Invincible");
            immune = true;
        } else if (Time.time > immuneTime && immune)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            immune = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        immuneTime = Time.time + 1f;
        if(health < 0)
        {
            SceneController.Instance.ReloadScene();
        }
    }

    public void SetHealAmount(float amount)
    {
        healAmount = amount;
    }

    public void SetHealRate(float rate)
    {
        healRate = rate;
    }
}
