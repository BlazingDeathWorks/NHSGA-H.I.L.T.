using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NavigationButton : MonoBehaviour
{
    public bool Unlocked { get; private set; }
    protected PlayerData PlayerData { get; set; }
    [SerializeField][Tooltip("If true, PNB will be unlocked at the beginning")] public bool DefaultButton = false;
    [SerializeField] protected CodeBlock LineOfCode;

    protected virtual void Awake()
    {
        if (DefaultButton || LineOfCode == null)
        {
            UnlockButton(true);
            return;
        }

        gameObject.SetActive(false);
        //Only do this if PlayerDataManager.LoadLoot == true
        //Do a for loop that checks the list of selected properties from the loaded data and if matched then UnlockButton
        //OnButtonClick() - Do all this in the overriding awake
    }

    public void UnlockButton(bool awake = false)
    {
        if (gameObject == null) return;
        PropertyNavigationButton pnb = (PropertyNavigationButton)this;
        if (awake || pnb.Parent == IDEManager.Instance.CurrentClass)
        {
            Debug.Log("Hwhy");
            gameObject?.SetActive(true);
        }
        Unlocked = true;
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
    public bool IsClicked { get; set; } = false;
    public Text Text => text;
    [SerializeField] private PropertyNavigationButton parentPropNavButton;
    [SerializeField] private Text[] childrenTextRecolor;
    [SerializeField] private string codeFragment;
    [SerializeField] private UnityEvent onCodeBlockEnabled;
    [SerializeField] private UnityEvent onCodeBlockDisabled;
    private Button button;
    private Text text;
    public PropertyNavigationButton DefaultButtonGameObject;

    protected override void Awake()
    {
        base.Awake();
        
        text = GetComponentInChildren<Text>();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.onClick.AddListener(() => IDEManager.Instance.SetCurrentClass(Parent));

        if (DefaultButton && PlayerDataManager.Instance.LoadLoot == false)
        {
            text.color = Color.yellow;
        }
        if (PlayerDataManager.Instance.LoadLoot)
        {
            PlayerData = PlayerDataManager.Instance.LoadPlayerData();
            IDEManager.Instance.SetCurrentClass(IDEManager.Instance.CurrentClass);
            IDEManager.Instance.ResetContent();
            foreach (string pnbName in PlayerData.PropertyNavigationButtons.Keys)
            {
                if (gameObject.name.Equals(pnbName))
                {
                    UnlockButton(true);
                    if (PlayerData.PropertyNavigationButtons[pnbName])
                    {
                        text.color = Color.yellow;
                        if (!DefaultButton) UpgradeLimiter.Instance.AddUpgrade();
                        OnButtonClick();
                    }
                    return;
                }
            }

        }

        //Else do a for loop that checks the list of selected properties from the loaded data and if matched then highlight
        //else
        //{
        //    for (int i = 0; i < PlayerData.PropertyNavigationButtons.Count; i++)
        //    {
        //        if (this == PlayerData.PropertyNavigationButtons[i])
        //        {
        //            text.color = Color.yellow;
        //            break;
        //        }
        //    }
        //}
    }

    //Nothing has to be added to Unity's Button Component OnClick()
    public void OnButtonClick()
    {
        if (IsClicked) return;
        Parent.SetActivatePNB(this);

        //Only set isClicked to true if we are currently on Class (Works because we're getting called before changing classes to this) - Basically ignore
        if (!LineOfCode) return;
        IsClicked = true;
        if(DefaultButton)
        {
            if(!LineOfCode.GetCodeFragment().Equals(codeFragment)) UpgradeLimiter.Instance.RemoveUpgrade();
        } else if (LineOfCode.GetCodeFragment().Equals(DefaultButtonGameObject.codeFragment))
        {
            if (UpgradeLimiter.Instance.atLimit)
            {
                UpgradeLimiter.Instance.PlayError();
                IsClicked = false;
                return;
            }
            UpgradeLimiter.Instance.AddUpgrade();
        }
        onCodeBlockEnabled.Invoke();
        LineOfCode?.ReplaceCodeFragment(codeFragment);

        //Only to list of objects
        //if (LineOfCode != null) text.color = new Color(240, 235, 216);
        if (parentPropNavButton != null)
        {
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
    }

    public void OnButtonUnClick()
    {
        onCodeBlockDisabled.Invoke();
        IsClicked = false;
    }
}