using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int _moveForwardSpeedHash = Animator.StringToHash("MovementForwardSpeed");
    private readonly int _moveRightSpeedHash = Animator.StringToHash("MovementRightSpeed");
    private readonly int _blockHash = Animator.StringToHash("BlockingBlendTree");
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {

        stateMachine.Animator.CrossFadeInFixedTime(_blockHash,CrossFadeDuration);
        stateMachine.ShieldCollider.SetActive(true);
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.AttackEvent += OnAttack;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine._isGrounded = stateMachine.Controller.isGrounded;
        MovePlayer(deltaTime,stateMachine.blockingSpeedMultiplier);
        RotateCamera(deltaTime);
        CalcMovementBlendTreeVals(deltaTime);
        PlayAudio();
        if (!stateMachine._isGrounded)
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false));
        }
        if (!stateMachine._isBlocking)
        {
            stateMachine.SwitchState(new PlayerMovementState(stateMachine));
        }
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

        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.ShieldCollider.SetActive(false);
    }
    private void OnAttack()
    {
        if (!GameGlobalVals.Instance.isGamePaused)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
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
        stateMachine.playerAudioSource.PlayOneShot(stateMachine.playerWalking);
       
    }
    private void OnJump()
    {
        if (!GameGlobalVals.Instance.isGamePaused)
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine, true));
        }
        
    }
 
}
