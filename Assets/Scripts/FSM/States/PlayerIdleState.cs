using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundAttackControllerState
{
    protected override string StringToHash => "Idle";

    public PlayerIdleState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.IsFalling)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
        }
        if (PlayerEntity.IsRunning)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerRunState);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Reverse One Way Player");
        }
    }
}
