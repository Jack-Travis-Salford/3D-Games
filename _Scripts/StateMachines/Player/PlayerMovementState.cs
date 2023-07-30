using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    private readonly int _movementBlendTreeHash = Animator.StringToHash("PlayerWalkingBlendTree");
    private readonly int _moveForwardSpeedHash = Animator.StringToHash("MovementForwardSpeed");
    private readonly int _moveRightSpeedHash = Animator.StringToHash("MovementRightSpeed");
    public PlayerMovementState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    { 

        stateMachine.Animator.CrossFadeInFixedTime(_movementBlendTreeHash,CrossFadeDuration);
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.AttackEvent += OnAttack;
    }

    public override void Tick(float deltaTime)
    {
       
        stateMachine._isGrounded = stateMachine.Controller.isGrounded;
        if (stateMachine._isRunning)
        {
            MovePlayer(deltaTime, stateMachine.runningSpeedMultiplier);
        }
        else
        {
            MovePlayer(deltaTime,1f);
        }
        RotateCamera(deltaTime);
        CalcMovementBlendTreeVals(deltaTime);
        PlayAudio();
        if (!stateMachine._isGrounded)
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine,false));
            return;
        }
        if (stateMachine._isBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
    }

    private void CalcMovementBlendTreeVals(float deltaTime)
    {
        float movementY = 0f;
        float movementX = 0f;
        switch (stateMachine.InputReader.MovementValue.y)
        {
            case (>0f):
                if (stateMachine._isRunning)
                {
                    movementY = 1f; 
                }
                else
                {
                    movementY = 0.5f;
                }
                    
                break;
            case (< 0f):
                if (stateMachine._isRunning)
                {
                    movementY = -1f; 
                }
                else
                {
                    movementY = -0.5f;
                }
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
    private void OnAttack()
    {
        if (!GameGlobalVals.Instance.isGamePaused)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
        }
        
    }

    private void OnJump()
    {
        if (!GameGlobalVals.Instance.isGamePaused)
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine, true));
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
        if (!stateMachine._isRunning)
        {
            stateMachine.playerAudioSource.PlayOneShot(stateMachine.playerWalking);
        }
        else
        {
            stateMachine.playerAudioSource.PlayOneShot(stateMachine.playerRunning);
        }
    }
    public override void Exit()
    {

        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.AttackEvent -= OnAttack;
    }
   
    
}
