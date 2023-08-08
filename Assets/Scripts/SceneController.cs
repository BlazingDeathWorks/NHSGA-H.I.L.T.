using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField]
    private AudioClip clickSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void NextScene()
    {
        AudioManager.Instance.PlayOneShot(clickSound);
        Time.timeScale = 1;
        FadeController.Instance.gameObject.SetActive(true);
        FadeController.Instance.FadeIn();
        StartCoroutine(ChangeScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void StartScene()
    {
        AudioManager.Instance.PlayOneShot(clickSound);
        Time.timeScale = 1;
        FadeController.Instance.gameObject.SetActive(true);
        FadeController.Instance.FadeIn();
        StartCoroutine(ChangeScene(0));
    }

    public void ReloadScene()
    {
        AudioManager.Instance.PlayOneShot(clickSound);
        Time.timeScale = 1;
        FadeController.Instance.gameObject.SetActive(true);
        FadeController.Instance.FadeIn();
        StartCoroutine(ChangeScene(SceneManager.GetActiveScene().buildIndex));
    }
    private IEnumerator ChangeScene(int index)
    {
        yield return new WaitForSecondsRealtime(1f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(index);
    }
}
