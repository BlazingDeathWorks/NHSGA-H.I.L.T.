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

    public SpriteRenderer flashRenderer;

    void Update()
    {
        if (flashRenderer.color.a > 0) {
            flashRenderer.color = new Color(.75f, .75f, .75f, flashRenderer.color.a - 2 * Time.deltaTime);
        }
    }

    public void OnHit()
    {
        if (CurrencyManager.Instance.Coins > cost)
        {
            //subtract from player
            CurrencyManager.Instance.AddCoins(-cost);

            //create number
            DamageNumber holdNum = Instantiate(damageTextPrefab, GameObject.Find("DamageNumbers").transform);
            holdNum.SetText(-cost);
            holdNum.SetColor(Color.yellow);
            holdNum.transform.position = transform.position + Vector3.up * 1.5f;

            //do lootcard
            //TODO
            Debug.Log("lmao");

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
