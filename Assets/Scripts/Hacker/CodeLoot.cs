using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.ShaderData;

public class CodeLoot : MonoBehaviour
{
    private List<Loot> loots = new();

    private void Start()
    {
        CodeLootManager.Instance?.GetRandomCollection(loots);
    }

    //Call when the enemy dies
    public NavigationButton Pull()
    {
        int randomIndex = Random.Range(0, loots.Count);

        for (int i = 0; i < loots[randomIndex].CheckSequence.Length; i++)
        {
            if (!loots[randomIndex].CheckSequence[i].CheckActive())
            {
                return null;
            }
        }
        return loots[randomIndex].NV;
        //loots[randomIndex].NV?.UnlockButton();
    }
}
