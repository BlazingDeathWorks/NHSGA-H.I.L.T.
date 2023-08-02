using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance { get; private set; }
    private int keysNeeded;
    private int keysCollected;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Open();
        }
    }

    private void Open()
    {
        if (keysCollected >= keysNeeded)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterNewKey()
    {
        keysNeeded++;
    }

    //Call when Enemy Dies
    public void CollectKey()
    {
        keysCollected++;
    }
}
