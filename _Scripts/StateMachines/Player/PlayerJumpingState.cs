using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int _jumpHash = Animator.StringToHash("jump");
    //Since falling using the same animation and very similar logic, both use this state.
    //This bool indicates which of the two caused the state change.
    //If jump, then true. If fell, then false
    private bool triggeredFromJump;
    //If entered state from fall, theres a slight delay before animation plays
    private float timeInState;
 
    public PlayerJumpingState(PlayerStateMachine stateMachine, bool triggeredFromJump) : base(stateMachine)
    {
        this.triggeredFromJump = triggeredFromJump;
    }

    public override void Enter()
    {
        stateMachine.InputReader.AttackEvent += OnAttack;
        if (triggeredFromJump)
        {
            stateMachine._verticalVelocity += stateMachine.JumpingForce;
            stateMachine.Animator.CrossFadeInFixedTime(_jumpHash, CrossFadeDuration);
        }
    }

    public override void Tick(float deltaTime)
    {
        if (!triggeredFromJump)
        {
            timeInState += deltaTime;
            if (timeInState >= 0.2f)
            {
                stateMachine.Animator.CrossFadeInFixedTime(_jumpHash, CrossFadeDuration);
                triggeredFromJump = true;
            }
        }
        stateMachine._isGrounded = stateMachine.Controller.isGrounded;
        MovePlayer(deltaTime,1f);
        RotateCamera(deltaTime);
        if (stateMachine._isGrounded)
        {
            if (stateMachine._isBlocking)
            {
                stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerMovementState(stateMachine));
            }
        }
        
    }

    public override void Exit()
    {
        stateMachine.InputReader.AttackEvent -= OnAttack;

    }
    private void OnAttack()
    {
        if (!GameGlobalVals.Instance.isGamePaused)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
        }
        
    }

}
