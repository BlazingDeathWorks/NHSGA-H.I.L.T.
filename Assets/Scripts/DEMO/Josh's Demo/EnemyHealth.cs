using Cinemachine;
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
    private bool isDead;
    //private float knockbackTimer;
    public float stunTime = 0;
    public float poisonTime = 0;
    public float poisonDamage = 0;
    public float knockback = 10;
    private Enemy enemyScript;
    private BossController bossScript;
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
        Weapon_ht.Instance.OnSetKnockback += SetKnockback;
        Weapon_ht.Instance.OnSetStunTime += SetStunTime;
        Weapon_ht.Instance.OnSetPoisonDamage += SetPoisonDamage;
        Weapon_ht.Instance.OnSetPoisonRate += SetPoisonTime;

        knockback = Weapon_ht.Instance.Knockback;
        stunTime = Weapon_ht.Instance.StunTime;
        poisonDamage = Weapon_ht.Instance.PoisonDamage;
        poisonTime = Weapon_ht.Instance.PoisonRate;

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        TryGetComponent(out enemyScript);
        TryGetComponent(out bossScript);
        if (enemyScript)
        {
            healthbar = Instantiate(healthBarPrefab, GameObject.Find("EnemyHealthbars").transform);
            flashRenderer = enemyScript.GetFlashRenderer();
        }
        if (bossScript)
        {
            healthbar = healthBarPrefab;
            flashRenderer = bossScript.GetFlashRenderer();
        }
        healthbar.minValue = 0;
        healthbar.maxValue = health;
        healthbar.value = health;
        hitParticles = GetComponentInChildren<ParticleSystem>();
    }
    void Update()
    {
        //do poison damage
        if (Time.time < poisonTimer && !isDead)
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
            if (enemyScript) enemyScript.enabled = true;
            if (bossScript) bossScript.enabled = true;
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
        if(enemyScript && healthbar) healthbar.transform.position = transform.position + new Vector3(transform.localScale.x * healthbarOffsetX, healthbarOffsetY);
    }
    private void FixedUpdate()
    {
        if (!onFixedUpdate) return;
        rb.AddForce(Vector2.right * knockback * Mathf.Sign(transform.position.x - player.transform.position.x), ForceMode2D.Impulse);
    }

    private void SetKnockback(float value)
    {
        knockback = value;
    }

    private void SetStunTime(float value)
    {
        stunTime = value;
    }

    private void SetPoisonDamage(float value)
    {
        poisonDamage = value;
    }

    private void SetPoisonTime(float value)
    {
        poisonTime = value;
    }

    private void ExecuteKnockback()
    {
        onUpdate = true;
        onFixedUpdate = true;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        DamageNumber holdNum = Instantiate(damageTextPrefab, GameObject.Find("Canvas").transform);
        holdNum.SetText(damage);
        holdNum.transform.position = transform.position + Vector3.up * 1.5f;
        health -= damage;
        flashRenderer.color = new Color(.75f, .75f, .75f, 1);
        ScreenShake.Instance.noise.m_AmplitudeGain = 2.5f;

        if (bossScript) bossScript.AddStagger();

        //set stun
        if (Time.time > stunCooldown)
        {
            stunTimer = Time.time + stunTime;
            if (enemyScript) enemyScript.enabled = false;
            if (bossScript) bossScript.enabled = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            stunCooldown = Time.time + 5f;
        }
        //set poison
        if(poisonDamage != 0) poisonTimer = Time.time + poisonTime;
        //set knockback
        ExecuteKnockback();
        
        if (health <= 0)
        {
            //Instantiate the loot object
            ScreenShake.Instance.noise.m_AmplitudeGain = 5;
            player.GetComponent<PlayerHealth>().Lifesteal();
            if (enemyScript)
            {
                LootCard instance = Instantiate(lootCard, transform.position, Quaternion.identity);
                instance.nb = codeLoot.Pull();
                enemyScript.Die();
            }
            if (bossScript) bossScript.Die();
            if (healthbar != null) Destroy(healthbar.gameObject);
            flashRenderer.color = new Color(0, 0, 0, 0);
            hitParticles.Play();
            isDead = true;
        }
    }

    public float GetPortionHealth()
    {
        return health / healthbar.maxValue;
    }

    public void MultiplyHealth(float val)
    {
        health *= val;
    }
}
