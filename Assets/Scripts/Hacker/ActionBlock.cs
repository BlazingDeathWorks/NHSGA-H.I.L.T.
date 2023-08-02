using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActionBlock : MonoBehaviour
{
    public bool Error { get; private set; } = true;
    private Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => Destroy(gameObject));
        button.onClick.AddListener(() =>
        {
            Ultimate_ht.Instance.Actions -= Execute;
            Ultimate_ht.Instance.ActionBlocks.Remove(this);
        });
    }

    private void Start()
    {
        Ultimate_ht.Instance.ActionBlocks.Add(this);
    }

    private void Update()
    {
        if (!Error) return;
        Error = CheckInputs();
    }

    protected abstract bool CheckInputs();

    public abstract void Execute(GameObject enemy);
}
