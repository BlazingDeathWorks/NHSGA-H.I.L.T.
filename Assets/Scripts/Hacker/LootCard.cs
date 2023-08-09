using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootCard : MonoBehaviour
{
    public NavigationButton Nb;
    public Vector2 ForceVector { get; set; }
    [SerializeField]
    private AudioClip pickupSound;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private bool launched;
    private bool oneFrame;

    private void Awake()
    {
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider.isTrigger = false;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(5f);
        boxCollider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        launched = false;
    }

    private void FixedUpdate()
    { 
        if (!launched && !oneFrame)
        {
            rb.AddForce(ForceVector, ForceMode2D.Impulse);
            oneFrame = true;
            launched = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (launched)
        {
            if (rb.velocity.y < 0 && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                boxCollider.isTrigger = true;
                gameObject.layer = LayerMask.NameToLayer("Invincible");
                launched = false;
                return;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Player") || (rb.velocity.y > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                return;
            }
            boxCollider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Invincible");
            launched = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayOneShot(pickupSound);
            PropertyNavigationButton propertyNavigationButton = (PropertyNavigationButton)Nb;
            if (propertyNavigationButton)
            {
                if(!propertyNavigationButton.isActiveAndEnabled) IDEManager.Instance.IncreaseMaxClamp();
                ConsoleManager.Instance.RequestMessage($"New Upgrade For {propertyNavigationButton?.Parent.GetComponentInChildren<Text>().text}");
            }
            else ConsoleManager.Instance.RequestMessage("Known or Sealed Code Found");
            Nb?.UnlockButton();
            Destroy(gameObject);
        }
    }
}
