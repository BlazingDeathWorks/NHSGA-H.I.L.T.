using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : State
{
    protected override string StringToHash => "Slide";
    private float timeSinceSlide;

    public PlayerSlideState(PlayerEntity playerEntity, FiniteStateMachine finiteStateMachine) : base(playerEntity, finiteStateMachine)
    {

    }

    public override void OnExit()
    {
        base.OnEnter();
        timeSinceSlide = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (PlayerEntity.IsFalling)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
        }

        timeSinceSlide += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftShift) || timeSinceSlide >= PlayerEntity.MaxSlideTime)
        {
            FiniteStateMachine.ChangeState(PlayerEntity.IsRunning ? PlayerEntity.PlayerRunState : PlayerEntity.PlayerIdleState);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        PlayerEntity.Rb.velocity = new Vector2(PlayerEntity.SlideSpeed * Mathf.Sign(PlayerEntity.gameObject.transform.localScale.x), 0);
    }
}
