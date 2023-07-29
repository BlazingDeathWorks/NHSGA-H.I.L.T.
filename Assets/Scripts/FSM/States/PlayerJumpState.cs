using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirAttackControllerState
{
    protected override string StringToHash => "Jump";
    private float timeSinceJump;

    public PlayerJumpState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.TimeSinceStartFall = PlayerEntity.CoyoteTime;
        timeSinceJump = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.IsFalling && !PlayerEntity.IsCoyoteTime)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            timeSinceJump += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            timeSinceJump = PlayerEntity.MaxJumpTime;
        }
        else
        {
            timeSinceJump = PlayerEntity.MaxJumpTime;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (PlayerEntity.IsJumping)
        {
            PlayerEntity.IsJumping = timeSinceJump < PlayerEntity.MaxJumpTime;
            PlayerEntity.IsCoyoteTime = PlayerEntity.IsJumping;
            PlayerEntity.Rb.velocity = new Vector2(PlayerEntity.Rb.velocity.x, PlayerEntity.JumpPower + timeSinceJump * 4);
        }
    }
}
