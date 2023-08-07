using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassNavigationButtonManager : MonoBehaviour
{
    [SerializeField] private ClassNavigationButton[] classNavButtons;

    private void Awake()
    {
        foreach(ClassNavigationButton nb in classNavButtons)
        {
            nb.Init();
        }
    }
}
