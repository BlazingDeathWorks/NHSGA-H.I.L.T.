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

    private GameObject minimap;
    private CursorLockMode prevMouseState;
    private bool minimapState;

    public void SetVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", .5f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxSlider.value) * 20);
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
    public void Activate(GameObject map, bool mapState)
    {
        minimap = map;
        minimapState = mapState;
        prevMouseState = Cursor.lockState;
        minimap.SetActive(false);
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        launchPanel.SetActive(true);
        Invoke("EnableSystem", .22f);
        Cursor.lockState = CursorLockMode.None;
    }
    public void SetInactive()
    {
        minimap.SetActive(minimapState);
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        launchPanel.SetActive(false);
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Cursor.lockState = prevMouseState;
    }
    public void EnableSystem()
    {
        if(gameObject.activeInHierarchy) Time.timeScale = 0;
        mainPanel.SetActive(true);
        launchPanel.SetActive(false);
    }
}
