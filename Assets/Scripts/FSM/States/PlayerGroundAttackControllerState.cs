using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Idle and Run as they can execute grounded attacks
public abstract class PlayerGroundAttackControllerState : State
{
    protected PlayerGroundAttackControllerState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.TimeSinceStartFall = 0;
        PlayerEntity.CanSlide = true;
    }

    public override void OnUpdate()
    {
        if (PlayerEntity.IsFalling)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
        }

        if (PlayerEntity.IsJumping)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerJumpState);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerSlideState);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Reverse One Way Player");
        }

        //STEP #1 - Create a new input for the new ground attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (PlayerEntity.TimeSinceLastBaseGroundAttack >= PlayerEntity.BaseGroundAttackCooldown)
            {
                PlayerEntity.TimeSinceLastBaseGroundAttack = 0;
                //STEP #2 - Change to that ground attack state
                FiniteStateMachine.ChangeState(PlayerEntity.PlayerBaseAttackState);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (PlayerEntity.TimeSinceLastSpecialGroundAttack >= PlayerEntity.SpecialGroundAttackCooldown)
            {
                PlayerEntity.TimeSinceLastSpecialGroundAttack = 0;
                FiniteStateMachine.ChangeState(PlayerEntity.PlayerThreeHitAttackState);
            }
        }
    }
}
