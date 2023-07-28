using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseAirAttackState : PlayerAirAttackState
{
    protected override string StringToHash => "BaseAirAttack";

    public PlayerBaseAirAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }
}
