using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDEManager : MonoBehaviour
{
    public static IDEManager Instance { get; private set; }
    public ClassNavigationButton CurrentClass { get => currentClass; }
    [SerializeField] private ClassNavigationButton currentClass;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetCurrentClass(ClassNavigationButton newClass)
    {
        if (newClass == CurrentClass) return;
        for (int i = 0; i < currentClass.LinesOfCode.Length; i++)
        {
            currentClass.LinesOfCode[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < newClass.LinesOfCode.Length; i++)
        {
            newClass.LinesOfCode[i].gameObject.SetActive(true);
        }
        currentClass = newClass;
    }
}
