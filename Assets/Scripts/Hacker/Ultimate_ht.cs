using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ultimate_ht : MonoBehaviour
{
    public static Ultimate_ht Instance { get; private set; }
    //Might change GameObject to something more specific later
    public event Action<GameObject> Actions;
    public List<ActionBlock> ActionBlocks { get; private set; } = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InvokeActions(GameObject enemy)
    {
        Actions?.Invoke(enemy);
    }

    public void Compile()
    {
        for (int i = 0; i < ActionBlocks.Count; i++)
        {
            Actions -= ActionBlocks[i].Execute;
        }
        ActionBlocks.Clear();

        for (int i = 0; i < ActionBlocks.Count; i++)
        {
            if (ActionBlocks[i] == null || ActionBlocks[i].Error) continue;
            Actions += ActionBlocks[i].Execute;
            Debug.Log("Successful Compiling");
        }
    }
}
