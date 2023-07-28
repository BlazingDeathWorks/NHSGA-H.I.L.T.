using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected abstract string StringToHash { get; }
    protected readonly int StateHash;

    protected readonly FiniteStateMachine FiniteStateMachine;
    protected readonly PlayerEntity PlayerEntity;

    public State(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine)
    {
        PlayerEntity = playerEntity;
        FiniteStateMachine = finiteStateMachine;
        StateHash = Animator.StringToHash(StringToHash);
    }

    //For Crossfade
    public virtual void OnEnter()
    {
        PlayerEntity.Animator.CrossFade(StateHash, 0, 0);
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
