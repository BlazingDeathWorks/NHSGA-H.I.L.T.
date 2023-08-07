using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUIManager : MonoBehaviour
{
    [SerializeField]
    private float coinChangeRate;
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //smooth coin change
        int coinCount = int.Parse(text.text);
        int goalCount = CurrencyManager.Instance.Coins;
        if (goalCount > coinCount)
        {
            coinCount += Mathf.CeilToInt(coinChangeRate * Time.deltaTime);
            if (coinCount > goalCount) coinCount = goalCount;
        }
        if (goalCount < coinCount)
        {
            coinCount -= Mathf.CeilToInt(coinChangeRate * Time.deltaTime);
            if (coinCount < goalCount) coinCount = goalCount;
        }

        text.text = ""+coinCount;
    }
}
