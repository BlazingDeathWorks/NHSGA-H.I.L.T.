using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLoot : MonoBehaviour
{
    private List<Loot> loots = new();

    private void Start()
    {
        CodeLootManager.Instance?.GetRandomCollection(loots);
    }

    //Call when the enemy dies
    public void Pull()
    {
        int randomIndex;
        bool pass;
        do
        {
            randomIndex = Random.Range(0, loots.Count);
            pass = true;

            for (int i = 0; i < loots[randomIndex].CheckSequence.Length; i++)
            {
                if (loots[randomIndex].CheckSequence[i].CheckShade())
                {
                    pass = false;
                    break;
                }
            }
        } while (pass == false);
        loots[randomIndex].PNB.UnlockButton();
    }
}
