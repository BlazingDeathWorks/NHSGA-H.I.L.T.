using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MockPropertyNavButton : MonoBehaviour
{
    [SerializeField] private Text lineOfCode;
    private const string STARTING_TABS = "\t\t";
    private const string UNCHANGED_CODE = "<color=#FF0094>Blade</color> currentBlade = <color=#FF0094>Blade</color>.";

    public void OnEnableButton(string bladeName)
    {
        lineOfCode.text = $"{STARTING_TABS}{UNCHANGED_CODE}{bladeName}";
    }
}
