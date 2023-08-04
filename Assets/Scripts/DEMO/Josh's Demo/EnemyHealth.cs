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
    private float stunTimer;
    private float stunCooldown;
    private float poisonTimer;
    private float knockbackTimer;
    public float stunTime = 3;
    public float poisonTime = 3;
    public float poisonDamage = 20;
    public float knockback = 20;
    private MeleeEnemy meleeScript;
    private RangedEnemy rangedScript;
    private TankEnemy tankScript;
    private Rigidbody2D rb;
    private GameObject player;

    private void Awake()
    {
        codeLoot = GetComponent<CodeLoot>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        healthbar = Instantiate(healthBarPrefab, GameObject.Find("EnemyHealthbars").transform);
        healthbar.minValue = 0;
        healthbar.maxValue = health;
        healthbar.value = health;
        TryGetComponent(out MeleeEnemy melee);
        if (melee != null) meleeScript = melee;
        TryGetComponent(out RangedEnemy ranged);
        if (ranged != null) rangedScript = ranged;
        TryGetComponent(out TankEnemy tank);
        if (tank != null) tankScript = tank;
    }
    void Update()
    {
        //do poison damage
        if (Time.time < poisonTimer)
        {
            health -= poisonDamage * Time.deltaTime;
            if (health < 0)
            {
                //Instantiate the loot object
                LootCard instance = Instantiate(lootCard, transform.position, Quaternion.identity);
                instance.nb = codeLoot.Pull();
                Destroy(gameObject);
            }
        }
        //reset stun
        if (Time.time > stunTimer)
        {
            if (meleeScript != null) meleeScript.enabled = true;
            if (rangedScript != null) rangedScript.enabled = true;
            if (tankScript != null) tankScript.enabled = true;
        }
        //do knockback
        if (Time.time < knockbackTimer)
        {
            rb.AddForce(Vector2.right * Mathf.Sign(transform.position.x - player.transform.position.x) * knockback, ForceMode2D.Force);
        }

        //smooth healthbar value change
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
        healthbar.transform.position = transform.position + new Vector3(transform.localScale.x * healthbarOffsetX, healthbarOffsetY);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        //set stun
        if (Time.time > stunCooldown)
        {
            stunTimer = Time.time + stunTime;
            if (meleeScript != null) meleeScript.enabled = false;
            if (rangedScript != null) rangedScript.enabled = false;
            if (tankScript != null) tankScript.enabled = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            stunCooldown = Time.time + 5f;
        }
        //set poison
        poisonTimer = Time.time + poisonTime;
        //set knockback
        knockbackTimer += .1f;
        
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
