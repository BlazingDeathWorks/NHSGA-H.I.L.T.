using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassNavigationButton : MonoBehaviour
{
    public CodeBlock[] LinesOfCode => linesOfCode;
    [SerializeField] private PropertyNavigationButton[] propertyNavigationButtons;
    [SerializeField] private CodeBlock[] linesOfCode;
    private Button button;
    private PropertyNavigationButton activePropNavButton;

    public void Init()
    {
        for (int i = 0; i < propertyNavigationButtons.Length; i++)
        {
            propertyNavigationButtons[i].Parent = this;
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(() => IDEManager.Instance.SetCurrentClass(this));
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
