using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public readonly List<string> PropertyNavigationButtons = new List<string>();

    public PlayerData(List<string> pnbs)
    {
        PropertyNavigationButtons = pnbs;
    }
}
