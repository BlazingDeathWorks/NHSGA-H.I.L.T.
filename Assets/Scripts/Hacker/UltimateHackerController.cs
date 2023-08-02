using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateHackerController : MonoBehaviour
{
    [SerializeField] private KeyCode hackerInterfaceOpenInput = KeyCode.Q;
    [SerializeField] private GameObject hackerIDE;

    private void Update()
    {
        if (Input.GetKeyDown(hackerInterfaceOpenInput))
        {
            if (hackerIDE.activeInHierarchy)
            {
                Ultimate_ht.Instance.Compile();
            }
            hackerIDE.SetActive(!hackerIDE.activeInHierarchy);
        }
    }
}
