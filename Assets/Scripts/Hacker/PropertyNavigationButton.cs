using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PropertyNavigationButton : MonoBehaviour
{
    public ClassNavigationButton Parent { get; set; }
    [SerializeField] private Image shade;
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

        if (defaultButton || lineOfCode == null)
        {
            UnlockButton();
            return;
        }
        shade.gameObject.SetActive(true);
        button.interactable = false;
    }

    public void UnlockButton()
    {
        shade.gameObject.SetActive(false);
        button.interactable = true;
    }

    //Nothing has to be added to Unity's Button Component OnClick()
    public void OnButtonClick()
    {
        if (isClicked) return;
        onCodeBlockEnabled.Invoke();
        lineOfCode?.ReplaceCodeFragment(codeFragment);
        Parent.SetActivatePNB(this);
        IDEManager.Instance.SetCurrentClass(Parent);
        isClicked = true;
    }

    public void OnButtonUnClick()
    {
        onCodeBlockDisabled.Invoke();
        isClicked = false;
    }

    public bool CheckShade()
    {
        return shade.gameObject.activeInHierarchy;
    }
}
