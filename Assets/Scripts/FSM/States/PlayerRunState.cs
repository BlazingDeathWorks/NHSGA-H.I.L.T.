using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundAttackControllerState
{
    protected override string StringToHash => "Run";

    public PlayerRunState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!PlayerEntity.IsRunning)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerIdleState);
        }
    }
}
