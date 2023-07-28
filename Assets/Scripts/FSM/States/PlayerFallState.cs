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

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        for (int i = 0; i < PlayerEntity.GroundRaycastPositions.Length; i++)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(PlayerEntity.GroundRaycastPositions[i].position, Vector2.up, PlayerEntity.PlatformRaycastDistance);
            if (hitInfo && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
    }
}
