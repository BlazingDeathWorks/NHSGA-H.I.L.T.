using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackState : State
{
    //Make sure this is sealed on sub abstract class
    protected abstract State TransitionState { get; }
    private AnimatorStateInfo animatorStateInfo;

    protected PlayerAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {
        animatorStateInfo = PlayerEntity.Animator.GetCurrentAnimatorStateInfo(0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.FinishedAttacking)
        {
            PlayerEntity.FinishedAttacking = false;
            FiniteStateMachine.ChangeState(TransitionState);
        }
    }
}
