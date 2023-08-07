using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this component to a GameObject and attach the main music to the audio source
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayOneShot(AudioClip clip, bool SFX = true)
    {
        if (SFX)
        {
            sfxSource.PlayOneShot(clip);
        } else
        {
            musicSource.PlayOneShot(clip);
        }
    }
}
