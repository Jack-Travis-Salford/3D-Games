using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private readonly int _moveForwardSpeedHash = Animator.StringToHash("MovementForwardSpeed");
    private readonly int _moveRightSpeedHash = Animator.StringToHash("MovementRightSpeed");
    private readonly int _attackHash = Animator.StringToHash("AttackingBlendTree");
    private AnimatorStateInfo currentAnimationInfo;
    private float passedTime =  0f;

    public PlayerAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(_attackHash, CrossFadeDuration);
        currentAnimationInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

    }

    public override void Tick(float deltaTime)
    {
        passedTime += deltaTime;
        stateMachine._isGrounded = stateMachine.Controller.isGrounded;
        MovePlayer(deltaTime,1f);
        RotateCamera(deltaTime);
        CalcMovementBlendTreeVals(deltaTime);
        PlayAudio();
        
        currentAnimationInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimationInfo.IsTag("attacking") && currentAnimationInfo.normalizedTime >= 0.9f)
        {
            if (!stateMachine._isGrounded)
            {
                stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false));
                return;
            }
            if (stateMachine._isBlocking)
            {
                stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerMovementState(stateMachine));
            }
        }
    }

    private void PlayAudio()
    {
        if (stateMachine.playerAudioSource.isPlaying)
        {
            return;
        }

        Vector2 playerMovement = stateMachine.InputReader.MovementValue;

        if (playerMovement == Vector2.zero)
        {
            return;
        }

        if (!stateMachine._isGrounded)
        {
            return;
        }
        stateMachine.playerAudioSource.PlayOneShot(stateMachine.playerWalking);
       
    }

    private void CalcMovementBlendTreeVals(float deltaTime)
    {
        float movementY = 0f;
        float movementX = 0f;
        switch (stateMachine.InputReader.MovementValue.y)
        { 
            case (> 0f):
                movementY = 1f;
                break;
            case (< 0f):
                movementY = -1f;
                break;
            default:
                movementY = 0f;
                break;
        }
        stateMachine.Animator.SetFloat(_moveForwardSpeedHash, movementY, AnimatorDampTime, deltaTime);
        switch (stateMachine.InputReader.MovementValue.x)
        {
            case (> 0f):
                movementX = 1f;
                break;
            case (< 0f):
                movementX = -1f;
                break;
            default:
                movementX = 0f;
                break;
        }
        stateMachine.Animator.SetFloat(_moveRightSpeedHash, movementX, AnimatorDampTime, deltaTime);
    }
    public override void Exit()
    {
    }
}
