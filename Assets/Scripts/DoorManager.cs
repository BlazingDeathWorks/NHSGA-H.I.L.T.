using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance { get; private set; }
    public List<PropertyNavigationButton> PNBS { get; private set; } = new List<PropertyNavigationButton>();
    [SerializeField] private ClassNavigationButton[] classes;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        for (int i = 0; i < classes.Length; i++)
        {
            for (int j = 0; j < classes[i].PropertyNavigationButtons.Length; j++)
            {
                PNBS.Add(classes[i].PropertyNavigationButtons[j]);
            }
        }
    }
}
