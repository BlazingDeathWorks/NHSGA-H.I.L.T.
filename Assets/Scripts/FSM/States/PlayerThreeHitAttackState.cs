using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThreeHitAttackState : PlayerGroundAttackState
{
    protected override string StringToHash => "ThreeHitAttack";

    public PlayerThreeHitAttackState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }
}
