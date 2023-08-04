using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCard : MonoBehaviour
{
    [HideInInspector] public NavigationButton nb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nb?.UnlockButton();
            Destroy(gameObject);
        }
    }
}
