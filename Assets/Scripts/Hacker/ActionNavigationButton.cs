using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionNavigationButton : NavigationButton
{
    [SerializeField] private RectTransform fileBar;
    [SerializeField] private ActionBlock actionBlockPrefab;
    private Button button;

    protected override void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CreateNewActionBlock);

        base.Awake();
    }

    private void CreateNewActionBlock()
    {
        ActionBlock instance = Instantiate(actionBlockPrefab, Vector2.zero, Quaternion.identity);
        instance.GetComponent<RectTransform>().SetParent(fileBar, false);
    }
}
