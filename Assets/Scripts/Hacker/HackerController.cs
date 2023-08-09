using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HackerController : MonoBehaviour
{
    [SerializeField] private KeyCode hackerInterfaceOpenInput = KeyCode.E;
    [SerializeField] private GameObject hackerIDE;
    [SerializeField] private GameObject miniMap;
    [SerializeField] private Volume volume;
    [SerializeField] private AudioSource HackMusic;
    [SerializeField] private PauseMenuController pauseMenu;

    private ChromaticAberration chromAb;
    private void Start()
    {
        volume.profile.TryGet(out chromAb);
        chromAb.active = !hackerIDE.activeInHierarchy;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKeyDown(hackerInterfaceOpenInput) && !pauseMenu.pausePanel.active)
        {
            hackerIDE.SetActive(!hackerIDE.activeInHierarchy);
            HackMusic.volume = hackerIDE.activeInHierarchy ? 1 : 0;
            Time.timeScale = hackerIDE.activeInHierarchy ? 0 : 1;
            IDEManager.Instance.SetCurrentClass(IDEManager.Instance.CurrentClass);
            IDEManager.Instance.ResetContent();
            AudioManager.Instance.MuteMusic(hackerIDE.activeInHierarchy);
            miniMap.SetActive(!hackerIDE.activeInHierarchy);
            chromAb.active = !hackerIDE.activeInHierarchy;
            Cursor.lockState = hackerIDE.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
