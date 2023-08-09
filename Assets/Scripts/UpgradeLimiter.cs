using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLimiter : MonoBehaviour
{
    public static UpgradeLimiter Instance { get; private set; }
    public bool atLimit;
    private Slider upgradeSlider;
    [SerializeField]
    private int upgradeCount;

    [SerializeField]
    private int upgradeLimit;
    [SerializeField]
    private AudioClip errorSound;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        upgradeSlider = GetComponent<Slider>();
        upgradeSlider.maxValue = upgradeLimit;
    }

    public void AddUpgrade()
    {
        upgradeCount++;
        upgradeSlider.value++;
        atLimit = upgradeSlider.value == upgradeLimit;
    }
    public void RemoveUpgrade()
    {
        upgradeCount--;
        upgradeSlider.value--;
        atLimit = false;
    }
    public void PlayError()
    {
        ConsoleManager.Instance.RequestMessage("Upgrade Limit Reached");
        AudioManager.Instance.PlayOneShot(errorSound);
    }
}
