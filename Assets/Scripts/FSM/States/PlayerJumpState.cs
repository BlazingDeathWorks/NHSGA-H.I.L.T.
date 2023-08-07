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
        //PlayerEntity.Rb.velocity = new Vector2(PlayerEntity.Rb.velocity.x, PlayerEntity.JumpPower);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.IsFalling && !PlayerEntity.IsCoyoteTime && !PlayerEntity.IsDoubleJumping)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
            PlayerEntity.IsJumping = false;
        }

        if (PlayerEntity.IsGrounded && timeSinceJump >= PlayerEntity.MaxJumpTime)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerIdleState);
            PlayerEntity.IsJumping = false;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            timeSinceJump += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
        {
            timeSinceJump = PlayerEntity.MaxJumpTime;
        }
        else
        {
            timeSinceJump = PlayerEntity.MaxJumpTime;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (PlayerEntity.IsJumping)
        {
            PlayerEntity.IsJumping = timeSinceJump < PlayerEntity.MaxJumpTime;
            PlayerEntity.IsCoyoteTime = PlayerEntity.IsJumping;
            PlayerEntity.IsDoubleJumping = PlayerEntity.IsJumping;
            PlayerEntity.Rb.velocity = new Vector2(PlayerEntity.Rb.velocity.x, PlayerEntity.JumpPower + timeSinceJump * 4);
        }
    }
}
