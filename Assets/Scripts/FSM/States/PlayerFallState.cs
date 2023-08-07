using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirAttackControllerState
{
    protected override string StringToHash => "Fall";

    public PlayerFallState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.Rb.gravityScale = 0;
        PlayerEntity.StartCoroutine(PlayerEntity.ReturnGravity());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (PlayerEntity.IsGrounded)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerIdleState);
        }

        PlayerEntity.TimeSinceStartFall += Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && PlayerEntity.TimeSinceStartFall < PlayerEntity.CoyoteTime)
        {
            PlayerEntity.IsCoyoteTime = true;
            PlayerEntity.IsJumping = true;
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerJumpState);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        for (int i = 0; i < PlayerEntity.PlatformRaycastPositions.Length; i++)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast((Vector2)PlayerEntity.PlatformRaycastPositions[i].position, Vector2.up, PlayerEntity.PlatformRaycastDistance);
            if (hitInfo && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
    }
}
