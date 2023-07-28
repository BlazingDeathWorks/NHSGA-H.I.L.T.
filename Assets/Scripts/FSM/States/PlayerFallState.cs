using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirAttackControllerState
{
    protected override string StringToHash => "Fall";

    public PlayerFallState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.IsGrounded)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerIdleState);
        }
    }
}
