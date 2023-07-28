using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Idle and Run as they can execute grounded attacks
public abstract class PlayerGroundAttackControllerState : State
{
    protected PlayerGroundAttackControllerState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnUpdate()
    {
        if (PlayerEntity.IsJumping)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerJumpState);
        }

        //STEP #1 - Create a new input for the new ground attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //STEP #2 - Change to that ground attack state
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerBaseAttackState);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerThreeHitAttackState);
        }
    }
}
