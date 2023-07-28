using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Idle and Run as they can execute attacks
public abstract class PlayerAttackState : State
{
    protected PlayerAttackState(FiniteStateMachine finiteStateMachine, PlayerEntity playerEntity) : base(finiteStateMachine, playerEntity)
    {

    }

    //protected override void OnUpdate()
    //{

    //}
}
