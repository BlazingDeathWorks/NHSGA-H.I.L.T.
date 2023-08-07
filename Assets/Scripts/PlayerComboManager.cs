using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerComboManager : MonoBehaviour
{
    public static PlayerComboManager Instance { get; private set; }
    [SerializeField]
    private AudioClip[] hitSounds;

    private GameObject player;
    private int comboStage;
    private float nextComboReset;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if(Time.time > nextComboReset)
        {
            comboStage = 0;
        }
    }

    public bool ComboAdd()
    {
        AudioManager.Instance.PlayOneShot(hitSounds[comboStage++]);
        nextComboReset = Time.time + 1.5f;
        if (comboStage == 5)
        {
            comboStage = 0;
            return true;
        }
        return false;
    }
}
