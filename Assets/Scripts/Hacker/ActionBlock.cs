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
            RemoveListener();
            Ultimate_ht.Instance.ActionBlocks.Remove(this);
        });
    }

    private void Start()
    {
        Ultimate_ht.Instance.ActionBlocks.Add(this);
    }

    private void Update()
    {
        Error = CheckInputs();
    }

    public void RemoveListener()
    {
        Ultimate_ht.Instance.Actions -= Execute;
    }

    protected abstract bool CheckInputs();

    public abstract void Execute(GameObject enemy);
}
