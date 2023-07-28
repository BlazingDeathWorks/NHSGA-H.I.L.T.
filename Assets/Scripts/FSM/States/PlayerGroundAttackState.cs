using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundAttackState : PlayerAttackState
{
    protected sealed override State TransitionState => PlayerEntity.PlayerIdleState;

    protected PlayerGroundAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
