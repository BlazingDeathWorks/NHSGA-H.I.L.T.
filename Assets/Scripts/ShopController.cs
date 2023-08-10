using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private DamageNumber damageTextPrefab;
    [SerializeField] private ParticleSystem coinParticles;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private int cost;
    [SerializeField] private LootCard lootCard;

    private GameObject player;
    private List<Loot> loots = new();
    public SpriteRenderer flashRenderer;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if (flashRenderer.color.a > 0) {
            flashRenderer.color = new Color(.75f, .75f, .75f, flashRenderer.color.a - 2 * Time.deltaTime);
        }
    }

    public void OnHit()
    {
        if (CurrencyManager.Coins >= cost)
        {
            //subtract from player
            CurrencyManager.Instance.AddCoins(-cost);

            //create number
            DamageNumber holdNum = Instantiate(damageTextPrefab, GameObject.Find("DamageNumbers").transform);
            holdNum.SetText(-cost);
            holdNum.SetColor(Color.yellow);
            holdNum.transform.position = transform.position + Vector3.up * 1.5f;

            //do lootcard
            CodeLootManager.Instance?.GetRandomCollection(loots);
            LootCard instance = Instantiate(lootCard, transform.position + Vector3.up, Quaternion.identity);
            List<Loot> newUpgrades = new List<Loot>();
            foreach (Loot getLoot in loots)
            {
                if (!getLoot.NV.Unlocked) newUpgrades.Add(getLoot);
            }
            if (newUpgrades.Count == 0) newUpgrades.Add(loots[0]);
            instance.Nb = newUpgrades[Random.Range(0, newUpgrades.Count)].NV;
            Physics2D.IgnoreCollision(instance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            instance.ForceVector = new Vector2(Mathf.Sign(transform.position.x - player.transform.position.x) * 3.5f, 13);

            coinParticles.Play();
            AudioManager.Instance.PlayOneShot(buySound);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(errorSound);
        }
        //hit effects
        flashRenderer.color = new Color(.75f, .75f, .75f, 1);
        ScreenShake.Instance.noise.m_AmplitudeGain = 3f;
    }
}
