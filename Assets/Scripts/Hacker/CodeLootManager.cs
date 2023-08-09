using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLootManager : MonoBehaviour
{
    public static CodeLootManager Instance { get; private set; }
    [SerializeField] private Loot[] loots;
    [SerializeField] private int min = 3, max = 5;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GetRandomCollection(List<Loot> loots)
    {
        int randomCollectionAmount = Random.Range(min, max + 1);
        List<int> indexesUsed = new();
        for (int i = 0; i < randomCollectionAmount; i++)
        {
            if (this.loots == null || this.loots.Length < randomCollectionAmount) return;
            int index = Random.Range(0, this.loots.Length);
            bool pass;
            do
            {
                pass = true;
                for (int j = 0; j < indexesUsed.Count; j++)
                {
                    if (indexesUsed[j] == index)
                    {
                        pass = false;
                        break;
                    }
                }
            } while (pass == false);
            loots.Add(this.loots[index]);
            indexesUsed.Add(index);
        }
    }
}

[System.Serializable]
public class Loot
{
    public NavigationButton NV => navigationButton;
    public NavigationButton[] CheckSequence => checkSequence;
    [SerializeField] private NavigationButton navigationButton;
    [SerializeField] private NavigationButton[] checkSequence;
}
