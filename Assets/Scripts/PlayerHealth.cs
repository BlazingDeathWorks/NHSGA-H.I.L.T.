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

    void Start()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = health;
        healthbar.value = health;
    }
    void Update()
    {
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
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health < 0)
        {
            Debug.Log("You died lol");
        }
    }
}
