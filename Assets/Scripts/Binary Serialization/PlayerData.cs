using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public readonly int Coins;
    public readonly List<PropertyNavigationButton> PropertyNavigationButtons;

    public PlayerData(int coins, List<PropertyNavigationButton> pnbs)
    {
        Coins = coins;
        PropertyNavigationButtons = pnbs;
    }
}
