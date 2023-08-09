using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassNavigationButton : MonoBehaviour
{
    public CodeBlock[] LinesOfCode => linesOfCode;
    public PropertyNavigationButton[] PropertyNavigationButtons => propertyNavigationButtons;
    [SerializeField] private PropertyNavigationButton[] propertyNavigationButtons;
    [SerializeField] private CodeBlock[] linesOfCode;
    private Button button;
    private PropertyNavigationButton activePropNavButton;

    private PropertyNavigationButton defaultButton;

    private void Start()
    {
        if (this == IDEManager.Instance.CurrentClass) return;
        for (int i = 0; i < LinesOfCode.Length; i++)
        {
            LinesOfCode[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < PropertyNavigationButtons.Length; i++)
        {
            PropertyNavigationButtons[i].gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        for (int i = 0; i < propertyNavigationButtons.Length; i++)
        {
            propertyNavigationButtons[i].Parent = this;
            if (propertyNavigationButtons[i].DefaultButton) defaultButton = propertyNavigationButtons[i];
            if (defaultButton) propertyNavigationButtons[i].DefaultButtonGameObject = defaultButton;
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(() => IDEManager.Instance.SetCurrentClass(this));
        button.onClick.AddListener(() => IDEManager.Instance.ResetContent());
    }

    public void SetActivatePNB(PropertyNavigationButton pnb)
    {
        activePropNavButton = pnb;
        for (int i = 0; i < propertyNavigationButtons.Length; i++)
        {
            if (propertyNavigationButtons[i] == activePropNavButton) continue;
            propertyNavigationButtons[i].OnButtonUnClick();
        }
    }
}
