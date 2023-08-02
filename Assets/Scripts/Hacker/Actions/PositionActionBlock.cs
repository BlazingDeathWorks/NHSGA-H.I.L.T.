using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionActionBlock : ActionBlock
{
    [SerializeField] private InputField firstInput;
    [SerializeField] private InputField secondInput;
    private float xPos, yPos;

    protected override bool CheckInputs()
    {
        if (!float.TryParse(firstInput.text, out xPos))
        {
            return true;
        }
        if (!float.TryParse(secondInput.text, out yPos))
        {
            return true;
        }
        return false;
    }

    public override void Execute(GameObject enemy)
    {
        enemy.transform.position = new Vector2(xPos, yPos);
    }
}
