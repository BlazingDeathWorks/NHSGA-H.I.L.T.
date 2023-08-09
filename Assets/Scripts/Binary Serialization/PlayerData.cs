using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public readonly Dictionary<string, bool> PropertyNavigationButtons = new Dictionary<string, bool>();

    public PlayerData(Dictionary<string, bool> pnbs)
    {
        PropertyNavigationButtons = pnbs;
    }
}
