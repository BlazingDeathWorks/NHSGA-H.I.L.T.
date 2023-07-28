using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseAttackState : PlayerGroundAttackState
{
    protected override string StringToHash => "BaseAttack";

    public PlayerBaseAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }
}
