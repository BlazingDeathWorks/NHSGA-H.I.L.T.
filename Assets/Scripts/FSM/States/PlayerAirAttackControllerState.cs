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

        if (Input.GetKeyDown(PlayerEntity.Abilities_ht.AbilityInput))
        {
            PlayerEntity.Abilities_ht.ExecuteAbility();
        }

        if (PlayerEntity.TimeSinceLastAirAttack >= PlayerEntity.AirAttackCooldown && PlayerEntity.TimeSinceLastAirAttackInput <= PlayerEntity.AirAttackBuffer)
        {
            PlayerEntity.TimeSinceLastAirAttack = 0;
            //STEP #2 - Change to that air attack state
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerBaseAirAttackState);
        }
    }
}
