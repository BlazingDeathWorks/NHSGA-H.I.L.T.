using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLimiter : MonoBehaviour
{
    public static UpgradeLimiter Instance { get; private set; }
    public bool atLimit;

    private int upgradeCount;

    [SerializeField]
    private int upgradeLimit;
    [SerializeField]
    private AudioClip errorSound;
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddUpgrade()
    {
        upgradeCount++;
        atLimit = upgradeCount == upgradeLimit;
    }
    public void RemoveUpgrade()
    {
        upgradeCount--;
        atLimit = false;
    }
    public void PlayError()
    {
        ConsoleManager.Instance.RequestMessage("Upgrade Limit Reached");
        AudioManager.Instance.PlayOneShot(errorSound);
    }
}
