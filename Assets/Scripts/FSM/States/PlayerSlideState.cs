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

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerEntity.CanSlide = false;
        if (PlayerEntity.IsGrounded) PlayerEntity.Rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
    }

    public override void OnExit()
    {
        base.OnEnter();
        timeSinceSlide = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        timeSinceSlide += Time.deltaTime;
        if (timeSinceSlide >= PlayerEntity.MaxSlideTime)
        {
            if (!PlayerEntity.IsGrounded)
            {
                FiniteStateMachine.ChangeState(PlayerEntity.PlayerFallState);
                return;
            }
            FiniteStateMachine.ChangeState(PlayerEntity.IsRunning ? PlayerEntity.PlayerRunState : PlayerEntity.PlayerIdleState);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        PlayerEntity.Rb.velocity = new Vector2(PlayerEntity.SlideSpeed * Mathf.Sign(PlayerEntity.gameObject.transform.localScale.x), PlayerEntity.Rb.velocity.y);
    }
}
