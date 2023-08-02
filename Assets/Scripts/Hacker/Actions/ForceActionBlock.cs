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
        enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
    }
}
