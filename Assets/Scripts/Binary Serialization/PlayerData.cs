using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public readonly List<GameObject> PropertyNavigationButtons = new List<GameObject>();

    public PlayerData(List<GameObject> pnbs)
    {
        PropertyNavigationButtons = pnbs;
    }
}
