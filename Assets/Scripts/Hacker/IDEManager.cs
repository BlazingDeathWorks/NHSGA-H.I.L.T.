using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDEManager : MonoBehaviour
{
    public static IDEManager Instance { get; private set; }
    public ClassNavigationButton CurrentClass { get => currentClass; }
    [SerializeField] private ClassNavigationButton currentClass;
    [SerializeField] private RectTransform content;
    [SerializeField] private float minClamp, maxClamp;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void IncreaseMaxClamp()
    {
        maxClamp += 130f;
    }

    public void ClampContent()
    {
        content.localPosition = new Vector3(content.localPosition.x, Mathf.Clamp(content.localPosition.y, minClamp, maxClamp), content.localPosition.z);
        //if (content.localPosition.y < minClamp || content.localPosition.y > maxClamp) Debug.Log("WTF");
    }

    public void ResetContent()
    {
        content.localPosition = new Vector3(content.localPosition.x, minClamp, content.localPosition.z);
    }

    public void SetCurrentClass(ClassNavigationButton newClass)
    {
        if (newClass == CurrentClass) return;
        for (int i = 0; i < currentClass.LinesOfCode.Length; i++)
        {
            currentClass.LinesOfCode[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentClass.PropertyNavigationButtons.Length; i++)
        {
            currentClass.PropertyNavigationButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < newClass.LinesOfCode.Length; i++)
        {
            newClass.LinesOfCode[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < newClass.PropertyNavigationButtons.Length; i++)
        {
            if (!newClass.PropertyNavigationButtons[i].Unlocked) continue;
            newClass.PropertyNavigationButtons[i].gameObject.SetActive(true);
        }
        currentClass = newClass;
    }
}
