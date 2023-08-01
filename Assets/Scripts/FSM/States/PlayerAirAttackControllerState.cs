using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Jump and Fall as they can execute air attacks
public abstract class PlayerAirAttackControllerState : State
{
    protected PlayerAirAttackControllerState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerSlideState);
        }

        //STEP #1 - Create a new input for the new air attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //STEP #2 - Change to that air attack state
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerBaseAirAttackState);
        }
    }
}
