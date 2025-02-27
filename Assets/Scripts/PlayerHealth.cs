using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Vector2 ForceVector { get; set; }
    [SerializeField]
    private float health;
    [SerializeField]
    private Slider healthbar;
    [SerializeField]
    private float healthbarSpeed;
    [SerializeField]
    private SpriteRenderer flashRenderer;
    [SerializeField]
    private DamageNumber damageTextPrefab;
    [SerializeField]
    private GameObject deathPanel;
    [SerializeField]
    private AudioClip loseSound;

    private float immuneTime;
    private bool immune;
    private SpriteRenderer playerSprite;
    private int immuneIndicatorDirection;
    private Rigidbody2D rb;
    private PlayerEntity playerEntity;
    public bool isDead;

    //Default - 0
    private float healAmount = 5;
    private bool knockback = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerEntity = GetComponent<PlayerEntity>();
    }

    void Start()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = health;
        healthbar.value = health;
        playerSprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        //move healthbar smoothly
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

        //control immunity
        if (Time.time > immuneTime && immune)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            immune = false;
        }

        //control flash
        flashRenderer.sprite = playerSprite.sprite;
        if (Time.time > immuneTime)
        {
            flashRenderer.color = new Color(1, 1, 1, 0);
        } else
        {
            flashRenderer.color = new Color(1, 1, 1, flashRenderer.color.a + 4 * Time.deltaTime * immuneIndicatorDirection);
            if(flashRenderer.color.a < 0)
            {
                flashRenderer.color = new Color(1, 1, 1, 0);
                immuneIndicatorDirection = 1;
            }
            if (flashRenderer.color.a > .5f)
            {
                flashRenderer.color = new Color(1, 1, 1, .5f);
                immuneIndicatorDirection = -1;
            }
        }

    }

    private void FixedUpdate()
    {
        if (knockback)
        {
            knockback = false;
            rb.AddForce(ForceVector, ForceMode2D.Impulse);
            if(health > 0) playerEntity.enabled = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if (immune || isDead) return;
        //do damage number
        DamageNumber holdNum = Instantiate(damageTextPrefab, GameObject.Find("DamageNumbers").transform);
        holdNum.SetText(damage);
        holdNum.SetColor(Color.red);
        holdNum.transform.position = transform.position + Vector3.up * 1.5f;

        //do screen shake
        ScreenShake.Instance.noise.m_AmplitudeGain = 5f;

        //sound
        GetComponent<PlayerAudioController>().PlayDamageSound();

        health -= damage;
        flashRenderer.color = new Color(1, 1, 1, 1);
        immuneIndicatorDirection = -1;
        immuneTime = Time.time + 1f;
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        immune = true;

        knockback = true;

        if(health <= 0)
        {
            rb.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            rb.gravityScale = 0;
            knockback = false;
            GetComponent<PlayerEntity>().enabled = false;
            isDead = true;
            healthbar.transform.parent.gameObject.SetActive(false);
            if (TryGetComponent(out Animator anim)) anim.CrossFade("Death", 0, 0);
        }
    }

    public void SetDead()
    {
        Time.timeScale = 0;
        CurrencyManager.Instance.ResetCoins();
        FadeController.Instance.gameObject.SetActive(true);
        FadeController.Instance.DoubleFade();

        StartCoroutine(SetDeathPanel());
    }
    private IEnumerator SetDeathPanel()
    {
        yield return new WaitForSecondsRealtime(1f);
        Cursor.lockState = CursorLockMode.None;
        AudioManager.Instance.StopMusic();
        if (GameObject.Find("Boss"))
        {
            foreach (AudioSource a in GameObject.Find("Boss").GetComponents<AudioSource>())
            {
                a.Stop();
            }
        }
        AudioManager.Instance.PlayOneShot(loseSound);
        ConsoleManager.Instance.gameObject.SetActive(false);
        deathPanel.SetActive(true);
    }

    public void SetHealAmount(float amount)
    {
        healAmount = amount;
    }

    public void Lifesteal()
    {
        if(healAmount > 0)
        {
            DamageNumber holdNum = Instantiate(damageTextPrefab, GameObject.Find("DamageNumbers").transform);
            holdNum.SetText(healAmount);
            holdNum.SetColor(Color.green);
            holdNum.transform.position = transform.position + Vector3.up * 1.5f;
        }
        health += healAmount;
        health = Mathf.Clamp(health, 0, healthbar.maxValue);
    }
}
