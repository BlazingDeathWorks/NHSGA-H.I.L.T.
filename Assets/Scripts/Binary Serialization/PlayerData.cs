using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public readonly List<PropertyNavigationButton> PropertyNavigationButtons = new List<PropertyNavigationButton>();

    public PlayerData(List<PropertyNavigationButton> pnbs)
    {
        PropertyNavigationButtons = pnbs;
    }
}
