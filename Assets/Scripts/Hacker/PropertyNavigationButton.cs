using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NavigationButton : MonoBehaviour
{
    [SerializeField][Tooltip("If true, PNB will be unlocked at the beginning")] private bool defaultButton = false;
    [SerializeField] protected CodeBlock LineOfCode;

    protected virtual void Awake()
    {
        if (defaultButton || LineOfCode == null)
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

    public bool CheckActive()
    {
        return gameObject.activeInHierarchy;
    }
}

public class PropertyNavigationButton : NavigationButton
{
    public ClassNavigationButton Parent { get; set; }
    [SerializeField] private string codeFragment;
    [SerializeField] private UnityEvent onCodeBlockEnabled;
    [SerializeField] private UnityEvent onCodeBlockDisabled;
    private bool isClicked = false;
    private Button button;

    protected override void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.onClick.AddListener(() => IDEManager.Instance.SetCurrentClass(Parent));

        base.Awake();
    }

    //Nothing has to be added to Unity's Button Component OnClick()
    public void OnButtonClick()
    {
        if (isClicked) return;
        onCodeBlockEnabled.Invoke();
        LineOfCode?.ReplaceCodeFragment(codeFragment);
        Parent.SetActivatePNB(this);
        isClicked = true;
    }

    public void OnButtonUnClick()
    {
        onCodeBlockDisabled.Invoke();
        isClicked = false;
    }
}
