using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UltimateHackerController : MonoBehaviour
{
    [SerializeField] private KeyCode hackerInterfaceOpenInput = KeyCode.Q;
    [SerializeField] private GameObject hackerIDE;
    [SerializeField] private GameObject miniMap;

    private void Update()
    {
        if (Input.GetKeyDown(hackerInterfaceOpenInput))
        {
            if (hackerIDE.activeInHierarchy)
            {
                Ultimate_ht.Instance.Compile();
            }
            hackerIDE.SetActive(!hackerIDE.activeInHierarchy);
            miniMap.SetActive(!hackerIDE.activeInHierarchy);
        }
    }
}
