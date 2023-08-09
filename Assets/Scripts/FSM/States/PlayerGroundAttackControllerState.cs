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
        if(PlayerEntity.Rb.velocity.y <= .1f) PlayerEntity.TimeSinceStartFall = 0;
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

        if (Input.GetKeyDown(PlayerEntity.Abilities_ht.AbilityInput))
        {
            PlayerEntity.Abilities_ht.ExecuteAbility();
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Reverse One Way Player");
        }

        //STEP #1 - Create a new input for the new ground attack
        if (PlayerEntity.TimeSinceLastBaseGroundAttack >= PlayerEntity.BaseGroundAttackCooldown && PlayerEntity.TimeSinceLastBaseGroundAttackInput <= PlayerEntity.BaseGroundAttackBuffer)
        {
            PlayerEntity.TimeSinceLastBaseGroundAttack = 0;
            //STEP #2 - Change to that ground attack state
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerBaseAttackState);
        }

        if (PlayerEntity.TimeSinceLastSpecialGroundAttack >= PlayerEntity.SpecialGroundAttackCooldown && PlayerEntity.TimeSinceLastSpecialAttackInput <= PlayerEntity.SpecialAttackBuffer)
        {
            PlayerEntity.TimeSinceLastSpecialGroundAttack = 0;
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerThreeHitAttackState);
        }
    }
}
