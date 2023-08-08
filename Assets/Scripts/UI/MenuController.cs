using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuController : MonoBehaviour
{
    [SerializeField]
    protected GameObject launchPanel;
    [SerializeField]
    protected AudioClip clickSound;
    public virtual void Start()
    {
        Time.timeScale = 1;
        launchPanel.SetActive(true);
        Invoke("EnableSystem", 2f);
    }
    public void EnableSystem()
    {
        launchPanel.SetActive(false);
    }
    public void QuitGame()
    {
        AudioManager.Instance.PlayOneShot(clickSound);
        Invoke("CloseGame", .5f);
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
