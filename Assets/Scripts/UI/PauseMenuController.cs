using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private KeyCode pauseMenuOpenInput = KeyCode.Escape;
    public PauseMenuManager pausePanel;
    [SerializeField] private GameObject minimap;
    [SerializeField] private AudioSource IDEMusic;
    private PlayerHealth healthScript;
    private void Start()
    {
        healthScript = GetComponent<PlayerHealth>();
        pausePanel.SetVolume();
    }
    private void Update()
    {
        if (Input.GetKeyDown(pauseMenuOpenInput))
        {
            if (healthScript.isDead) return;
            if (!pausePanel.isActiveAndEnabled)
            {
                pausePanel.gameObject.SetActive(true);
                pausePanel.Activate(minimap, minimap.activeInHierarchy, IDEManager.Instance.gameObject, 
                    IDEManager.Instance.gameObject.activeInHierarchy, IDEMusic);
            }
        }
    }
}
