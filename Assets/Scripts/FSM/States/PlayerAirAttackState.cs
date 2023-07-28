using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAirAttackState : PlayerAttackState
{
    protected sealed override State TransitionState => PlayerEntity.PlayerFallState;

    public PlayerAirAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.Rb.velocity = Vector2.zero;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
