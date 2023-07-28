using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    protected State CurrentState { get; private set; }

    public void Initialize(State state)
    {
        CurrentState = state;
        CurrentState.OnEnter();
    }

    public void ChangeState(State state)
    {
        CurrentState.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();
    }
}
