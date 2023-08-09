using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MenuController
{
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
    [SerializeField]
    private Toggle modeToggle;

    public override void Start()
    {
        base.Start();
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", .5f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxSlider.value) * 20);

        PlayerPrefs.SetInt("mode", PlayerPrefs.GetInt("mode", 0));
        modeToggle.isOn = PlayerPrefs.GetInt("mode") == 1;
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
    public void SetMode(bool mode)
    {
        PlayerPrefs.SetInt("mode", mode ? 1 : 0);
    }
}
