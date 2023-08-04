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
    [SerializeField]
    private DamageNumber damageTextPrefab;

    private float timeSinceExecuted;
    private bool onUpdate = false;
    private bool onFixedUpdate = false;

    private Slider healthbar;
    private float stunTimer;
    private float stunCooldown;
    private float poisonTimer;
    private float knockbackTimer;
    public float stunTime = 0;
    public float poisonTime = 0;
    public float poisonDamage = 0;
    public float knockback = 10;
    private Enemy enemyScript;
    private Rigidbody2D rb;
    private GameObject player;
    private SpriteRenderer flashRenderer;
    private ParticleSystem hitParticles;

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
        enemyScript = GetComponent<Enemy>();
        flashRenderer = enemyScript.GetFlashRenderer();
        hitParticles = GetComponentInChildren<ParticleSystem>();
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
            enemyScript.enabled = true;
        }
        //do knockback
        if (onUpdate)
        {
            timeSinceExecuted += Time.deltaTime;
            if (timeSinceExecuted >= 0.1f)
            {
                timeSinceExecuted = 0;
                onUpdate = false;
                onFixedUpdate = false;
            }
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
        healthbar.transform.position = transform.position + new Vector3(transform.localScale.x * healthbarOffsetX, healthbarOffsetY);
    }
    private void FixedUpdate()
    {
        if (!onFixedUpdate) return;
        rb.AddForce(Vector2.right * knockback * Mathf.Sign(transform.position.x - player.transform.position.x), ForceMode2D.Impulse);
    }

    private void ExecuteKnockback()
    {
        onUpdate = true;
        onFixedUpdate = true;
    }

    public void TakeDamage(float damage)
    {
        DamageNumber holdNum = Instantiate(damageTextPrefab, GameObject.Find("EnemyHealthbars").transform);
        holdNum.SetText(damage);
        holdNum.transform.position = transform.position + Vector3.up * 1.5f;
        health -= damage;
        flashRenderer.color = new Color(.75f, .75f, .75f, 1);

        //set stun
        if (Time.time > stunCooldown)
        {
            stunTimer = Time.time + stunTime;
            enemyScript.enabled = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            stunCooldown = Time.time + 5f;
        }
        //set poison
        poisonTimer = Time.time + poisonTime;
        //set knockback
        ExecuteKnockback();
        
        if (health < 0)
        {
            //Instantiate the loot object
            LootCard instance = Instantiate(lootCard, transform.position, Quaternion.identity);
            instance.nb = codeLoot.Pull();
            enemyScript.Die();
            if(healthbar != null) Destroy(healthbar.gameObject);
            enabled = false;
            flashRenderer.color = new Color(0, 0, 0, 0);
            hitParticles.Play();
        }
    }

    public void MultiplyHealth(float val)
    {
        health *= val;
    }
}
