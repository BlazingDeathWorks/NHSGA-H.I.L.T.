using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ForceActionBlock : ActionBlock
{
    [SerializeField] private InputField firstInput;
    [SerializeField] private InputField secondInput;
    private float xForce, yForce;
    private EnemyController controller;
    private Rigidbody2D rb;
    private float timeSinceExecuted;

    private void OnUpdate()
    {
        if (controller == null) return;

        timeSinceExecuted += Time.deltaTime;
        if (timeSinceExecuted >= 0.2)
        {
            controller.enabled = true;
            timeSinceExecuted = 0;
            Ultimate_ht.Instance.OnUpdate -= OnUpdate;
            Ultimate_ht.Instance.OnFixedUpdate -= OnFixedUpdate;
        }
    }

    private void OnFixedUpdate()
    {
        if (rb == null) return;
        rb.velocity = new Vector2(xForce, yForce);
    }

    protected override bool CheckInputs()
    {
        if (!float.TryParse(firstInput.text, out xForce))
        {
            return true;
        }
        if (!float.TryParse(secondInput.text, out yForce))
        {
            return true;
        }
        return false;
    }

    public override void Execute(GameObject enemy)
    {
        controller = enemy.GetComponent<EnemyController>();
        rb = enemy.GetComponent<Rigidbody2D>();
        controller.enabled = false;

        Ultimate_ht.Instance.OnUpdate += OnUpdate;
        Ultimate_ht.Instance.OnFixedUpdate += OnFixedUpdate;
        Debug.Log("Registered");
    }
}
