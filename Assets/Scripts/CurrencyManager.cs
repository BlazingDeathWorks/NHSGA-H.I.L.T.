using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public int Coins { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCoins(int value)
    {
        StartCoroutine(AddCoinsToCounter(value));
    }
    private IEnumerator AddCoinsToCounter(int value)
    {
        yield return new WaitForSecondsRealtime(.5f);
        Coins += value;
    }

    public void ResetCoins()
    {
        Coins = 0;
    }
}
