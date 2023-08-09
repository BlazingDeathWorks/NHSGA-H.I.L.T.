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
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void MuteMusic(bool shouldMute)
    {
        musicSource.volume = shouldMute ? 0 : 1;
    }
    public void PlaySong(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }
    public void PlayOneShot(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
