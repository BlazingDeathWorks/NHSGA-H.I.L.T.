using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private KeyCode pauseMenuOpenInput = KeyCode.Escape;
    [SerializeField] private PauseMenuManager pausePanel;
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject ide;
    private void Start()
    {
        pausePanel.SetVolume();
    }
    private void Update()
    {
        if (Input.GetKeyDown(pauseMenuOpenInput))
        {
            if (!pausePanel.isActiveAndEnabled)
            {
                pausePanel.gameObject.SetActive(true);
                pausePanel.Activate(minimap, minimap.activeInHierarchy, ide, ide.activeInHierarchy);
            }
        }
    }
}
