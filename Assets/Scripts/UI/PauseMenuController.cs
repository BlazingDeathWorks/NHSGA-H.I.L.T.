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
    private void Start()
    {
        pausePanel.gameObject.SetActive(true);
        pausePanel.Activate(minimap, minimap.activeInHierarchy);
        pausePanel.SetInactive();
    }
    private void Update()
    {
        if (Input.GetKeyDown(pauseMenuOpenInput))
        {
            if (pausePanel.isActiveAndEnabled)
            {
                pausePanel.SetInactive();
            } else
            {
                pausePanel.gameObject.SetActive(true);
                pausePanel.Activate(minimap, minimap.activeInHierarchy);
            }
        }
    }
}
