using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject launchPanel;
    [SerializeField]
    private AudioClip clickSound;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    public bool active;

    private GameObject minimap;
    private CursorLockMode prevMouseState;
    private bool minimapState;
    private GameObject ide;
    private bool ideState;
    private AudioSource IDEMusic;
    private float IDEVolume;
    private float prevTimeScale;

    public void SetVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", .5f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxSlider.value) * 20);
    }
    public void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            minimap?.SetActive(false);
            ide?.SetActive(false);
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    public void SetPanelMain()
    {
        AudioManager.Instance.PlayOneShot(clickSound);
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void SetPanelOptions()
    {
        AudioManager.Instance.PlayOneShot(clickSound);
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void Activate(GameObject map, bool mapState, GameObject id, bool idState, AudioSource ideMusic)
    {
        minimap = map;
        minimapState = mapState;
        ide = id;
        ideState = idState;
        IDEMusic = ideMusic;
        IDEVolume = IDEMusic.volume;
        IDEMusic.volume = 0;
        prevTimeScale = Time.timeScale;
        prevMouseState = Cursor.lockState;
        minimap.SetActive(false);
        ide.SetActive(false);
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        launchPanel.SetActive(true);
        StartCoroutine(EnableSystem());
        AudioManager.Instance.PlayOneShot(clickSound);
        active = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void SetInactive()
    {
        minimap.SetActive(minimapState);
        ide.SetActive(ideState);
        IDEMusic.volume = IDEVolume;
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        launchPanel.SetActive(false);
        Time.timeScale = prevTimeScale;
        gameObject.SetActive(false);
        Cursor.lockState = prevMouseState;
        AudioManager.Instance.PlayOneShot(clickSound);
        active = false;
    }
public IEnumerator EnableSystem()
    {
        yield return new WaitForSecondsRealtime(.22f);
        if (gameObject.activeInHierarchy) Time.timeScale = 0;
        mainPanel.SetActive(true);
        launchPanel.SetActive(false);
    }
}
