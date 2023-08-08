using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }
    private Animator anim;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        anim.Play("FadeIn");
    }
    public void DoubleFade()
    {
        anim.Play("DoubleFade");
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
