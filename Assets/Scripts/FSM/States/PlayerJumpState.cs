using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirAttackControllerState
{
    protected override string StringToHash => "Jump";

    public PlayerJumpState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.TimeSinceStartFall = PlayerEntity.CoyoteTime;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.IsFalling && !PlayerEntity.IsCoyoteTime)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (PlayerEntity.IsJumping)
        {
            PlayerEntity.IsJumping = false;
            PlayerEntity.IsCoyoteTime = false;
            PlayerEntity.Rb.velocity = new Vector2(PlayerEntity.Rb.velocity.x, PlayerEntity.JumpPower);
        }
    }
}
