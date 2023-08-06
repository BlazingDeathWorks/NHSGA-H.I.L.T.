using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackState : State
{
    //Make sure this is sealed on sub abstract class
    protected abstract State TransitionState { get; }

    protected PlayerAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.CanMove = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerSlideState);
        }

        if (PlayerEntity.FinishedAttacking)
        {
            PlayerEntity.FinishedAttacking = false;
            FiniteStateMachine.ChangeState(TransitionState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        PlayerEntity.CanMove = true;
    }
}
