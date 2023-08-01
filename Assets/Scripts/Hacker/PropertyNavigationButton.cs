using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PropertyNavigationButton : MonoBehaviour
{
    public ClassNavigationButton Parent { get; set; }
    [SerializeField] [Tooltip("If true, PNB will be unlocked at the beginning")] private bool defaultButton = false;
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
        button.onClick.AddListener(() => IDEManager.Instance.SetCurrentClass(Parent));

        if (defaultButton || lineOfCode == null)
        {
            UnlockButton();
            return;
        }
        gameObject.SetActive(false);
    }

    public void UnlockButton()
    {
        gameObject.SetActive(true);
    }

    //Nothing has to be added to Unity's Button Component OnClick()
    public void OnButtonClick()
    {
        if (isClicked) return;
        onCodeBlockEnabled.Invoke();
        lineOfCode?.ReplaceCodeFragment(codeFragment);
        Parent.SetActivatePNB(this);
        isClicked = true;
    }

    public void OnButtonUnClick()
    {
        onCodeBlockDisabled.Invoke();
        isClicked = false;
    }

    public bool CheckActive()
    {
        return gameObject.activeInHierarchy;
    }
}
