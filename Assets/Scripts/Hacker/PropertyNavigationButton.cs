using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NavigationButton : MonoBehaviour
{
    [SerializeField][Tooltip("If true, PNB will be unlocked at the beginning")] protected bool DefaultButton = false;
    [SerializeField] protected CodeBlock LineOfCode;

    protected virtual void Awake()
    {
        if (DefaultButton || LineOfCode == null)
        {
            UnlockButton();
            return;
        }
        gameObject.SetActive(false);
    }

    public void UnlockButton()
    {
        if (gameObject == null) return;
        gameObject?.SetActive(true);
    }

    public bool CheckActive()
    {
        if (gameObject == null) return true;
        return gameObject.activeSelf;
    }
}

public class PropertyNavigationButton : NavigationButton
{
    public ClassNavigationButton Parent { get; set; }
    public Text[] ChildrenTextRecolor => childrenTextRecolor;
    [SerializeField] private PropertyNavigationButton parentPropNavButton;
    [SerializeField] private Text[] childrenTextRecolor;
    [SerializeField] private string codeFragment;
    [SerializeField] private UnityEvent onCodeBlockEnabled;
    [SerializeField] private UnityEvent onCodeBlockDisabled;
    private bool isClicked = false;
    private Button button;
    private Text text;

    protected override void Awake()
    {
        text = GetComponentInChildren<Text>();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.onClick.AddListener(() => IDEManager.Instance.SetCurrentClass(Parent));

        if (DefaultButton)
        {
            text.color = Color.yellow;
        }

        base.Awake();
    }

    //Nothing has to be added to Unity's Button Component OnClick()
    public void OnButtonClick()
    {
        if (isClicked) return;
        onCodeBlockEnabled.Invoke();
        LineOfCode?.ReplaceCodeFragment(codeFragment);
        Parent.SetActivatePNB(this);

        //Only set isClicked to true if we are currently on Class (Works because we're getting called before changing classes to this)
        if (IDEManager.Instance.CurrentClass != Parent) return;
        isClicked = true;

        //Only to list of objects
        //if (LineOfCode != null) text.color = new Color(240, 235, 216);
        if (parentPropNavButton == null) return;
        for (int i = 0; i < parentPropNavButton.ChildrenTextRecolor.Length; i++)
        {
            if (parentPropNavButton.ChildrenTextRecolor[i] == text)
            {
                text.color = Color.yellow;
                continue;
            }
            parentPropNavButton.ChildrenTextRecolor[i].color = new Color(240, 235, 216);
        }
    }

    public void OnButtonUnClick()
    {
        onCodeBlockDisabled.Invoke();
        isClicked = false;
    }
}