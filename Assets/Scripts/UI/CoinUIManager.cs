using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUIManager : MonoBehaviour
{
    [SerializeField]
    private float coinChangeRate;
    private TextMeshProUGUI text;
    private AudioSource coinSound;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "" + CurrencyManager.Instance.Coins;
        coinSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        //smooth coin change
        int coinCount = int.Parse(text.text);
        int goalCount = CurrencyManager.Instance.Coins;
        if (goalCount > coinCount)
        {
            if (!coinSound.isPlaying) coinSound.Play();
            coinCount += Mathf.CeilToInt(coinChangeRate * Time.deltaTime * goalCount - coinCount > 200 ? 2 : 1);
            if (coinCount > goalCount) coinCount = goalCount;
        }
        if (goalCount < coinCount)
        {
            if (!coinSound.isPlaying) coinSound.Play();
            coinCount -= Mathf.CeilToInt(coinChangeRate * Time.deltaTime * coinCount - goalCount > 200 ? 2 : 1);
            if (coinCount < goalCount) coinCount = goalCount;
        }
        if (coinCount == goalCount) coinSound.Stop();

        text.text = ""+coinCount;
    }
}
