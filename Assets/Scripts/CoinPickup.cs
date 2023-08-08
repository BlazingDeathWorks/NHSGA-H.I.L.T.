using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    private int value;
    [SerializeField]
    private AudioClip pickupSound;
    [SerializeField]
    private Transform minimapIcon;
    private SpriteRenderer sprite;
    private bool pickedUp;
    private float scaleChange;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        scaleChange = -1f;
    }
    private void Update()
    {
        if (pickedUp)
        {
            sprite.color = new Color(1, 1, 1, sprite.color.a - 2f * Time.deltaTime);
            if (sprite.color.a < 0) Destroy(gameObject);
        }

        transform.localScale = new Vector3(transform.localScale.x + scaleChange * Time.deltaTime, 1, 1);
        if(transform.localScale.x == 0) transform.localScale = new Vector3(scaleChange * Time.deltaTime, 1, 1);
        if (transform.localScale.x > 1)
        {
            scaleChange = -1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (transform.localScale.x < -1)
        {
            scaleChange = 1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        minimapIcon.localScale = new Vector3(1/transform.localScale.x, 1, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp) return;
        CurrencyManager.Instance.AddCoins(value);
        AudioManager.Instance.PlayOneShot(pickupSound);
        pickedUp = true;
    }
}
