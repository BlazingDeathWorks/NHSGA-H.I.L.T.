using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    //Return with Animator.StringToHash(StateName)
    protected abstract int StateHashName { get; }

    protected readonly FiniteStateMachine FiniteStateMachine;
    protected readonly PlayerEntity PlayerEntity;

    public State(FiniteStateMachine finiteStateMachine, PlayerEntity playerEntity)
    {
        FiniteStateMachine = finiteStateMachine;
        PlayerEntity = playerEntity;
    }

    //For Crossfade
    public virtual void OnEnter()
    {
        PlayerEntity.Animator.CrossFade(StateHashName, 0, 0);
    }

    //Reset Parameters
    public virtual void OnExit()
    {

    }

    //Executed on MonoBehaviour Update
    public virtual void OnUpdate()
    {

    }

    //Executed on MonoBehaviour FixedUpdate
    public virtual void OnFixedUpdate()
    {

    }
}
