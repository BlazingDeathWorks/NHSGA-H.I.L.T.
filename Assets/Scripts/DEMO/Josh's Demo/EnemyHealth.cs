using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float healthbarSpeed;
    [SerializeField] private LootCard lootCard;
    private CodeLoot codeLoot;
    [SerializeField]
    private Slider healthBarPrefab;
    [SerializeField]
    private float healthbarOffsetX;
    [SerializeField]
    private float healthbarOffsetY;

    private Slider healthbar;

    private void Awake()
    {
        codeLoot = GetComponent<CodeLoot>();
    }

    void Start()
    {
        healthbar = Instantiate(healthBarPrefab, GameObject.Find("EnemyHealthbars").transform);
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
    private void LateUpdate()
    {
        Invoke(nameof(UpdateHealthBar), 0.001f);
    }

    private void OnDestroy()
    {
        if(healthbar != null) Destroy(healthbar.gameObject);
    }

    private void UpdateHealthBar()
    {
        healthbar.transform.position = transform.position;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            //Instantiate the loot object
            LootCard instance = Instantiate(lootCard, transform.position, Quaternion.identity);
            instance.nb = codeLoot.Pull();
            Destroy(gameObject);
        }
    }

    public void MultiplyHealth(float val)
    {
        health *= val;
    }
}
