using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PropertyNavigationButton : MonoBehaviour
{
    [SerializeField] private string codeFragment;
    [SerializeField] private CodeBlock lineOfCode;
    [SerializeField] private UnityEvent onCodeBlockEnabled;
    [SerializeField] private UnityEvent onCodeBlockDisabled;
    private bool isClicked = false;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void UnlockButton()
    {
        button.interactable = true;
    }

    //Nothing has to be added to Unity's Button Component OnClick()
    public void OnButtonClick()
    {
        if (isClicked) return;
        onCodeBlockEnabled.Invoke();
        lineOfCode?.ReplaceCodeFragment(codeFragment);
        isClicked = true;
    }

    public void OnButtonUnClick()
    {
        onCodeBlockDisabled.Invoke();
        isClicked = false;
    }
}
