using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : MonoBehaviour
{
    [SerializeField] private KeyCode hackerInterfaceOpenInput = KeyCode.E;
    [SerializeField] private GameObject hackerIDE;
    [SerializeField] private GameObject miniMap;

    private void Update()
    {
        if (Input.GetKeyDown(hackerInterfaceOpenInput))
        {
            hackerIDE.SetActive(!hackerIDE.activeInHierarchy);
            miniMap.SetActive(!hackerIDE.activeInHierarchy);
        }
    }
}
