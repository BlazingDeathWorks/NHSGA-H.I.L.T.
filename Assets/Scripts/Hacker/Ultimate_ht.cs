using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ultimate_ht : MonoBehaviour
{
    public static Ultimate_ht Instance { get; private set; }
    //Remote Events
    public event Action OnUpdate;
    public event Action OnFixedUpdate;
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

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }

    public void InvokeActions(GameObject enemy)
    {
        Actions?.Invoke(enemy);
    }

    public void Compile()
    {
        for (int i = 0; i < ActionBlocks.Count; i++)
        {
            ActionBlocks[i].RemoveListener();
        }

        for (int i = 0; i < ActionBlocks.Count; i++)
        {
            if (ActionBlocks[i] == null || ActionBlocks[i].Error) continue;
            Actions += ActionBlocks[i].Execute;
            Debug.Log("Successful Compiling");
        }
    }
}
