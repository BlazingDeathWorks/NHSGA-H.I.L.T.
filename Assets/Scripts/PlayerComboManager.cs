using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this component to a GameObject and attach the main music to the audio source
[RequireComponent(typeof(AudioSource))]
public class PlayerComboManager : MonoBehaviour
{
    public static PlayerComboManager Instance { get; private set; }
    [SerializeField]
    private static AudioClip[] hitSounds;

    private GameObject player;
    private static int comboStage;
    private static float nextComboReset;

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

    public static bool ComboAdd()
    {
        AudioManager.Instance.PlayOneShot(hitSounds[comboStage++]);
        if(comboStage == 4)
        {
            comboStage = 0;
            return true;
        }
        return false;
    }
}
