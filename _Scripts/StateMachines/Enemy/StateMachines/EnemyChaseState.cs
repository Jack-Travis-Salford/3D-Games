using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyChaseState : EnemyBaseState
{
    //Moves enemy to the target distance away from the player
    private int _RunHash = Animator.StringToHash("EnemyRun");
    private const float CrossFadeDuration = 0.1f;

    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        stateMachine.Agent.isStopped = false;
        stateMachine.Agent.updateRotation = true;
        stateMachine.Agent.updatePosition = true;
        stateMachine.Animator.CrossFadeInFixedTime(_RunHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        _isGrounded = stateMachine.Controller.isGrounded;
        if (!ShouldContinueChase())
        {
            stateMachine.SwitchState(new EnemyDefensiveState(stateMachine));
            return;
        }
        stateMachine.Agent.destination = Player.transform.position;
        Vector3 movement = stateMachine.Agent.desiredVelocity.normalized * (stateMachine.MovementSpeed*1.5f);
        movement.y = 0f;
        Move(movement,deltaTime);
        PlayAudio();
    }
    private void PlayAudio()
    {
        if (stateMachine.EnemyAudioSource.isPlaying)
        {
            return;
        }
        stateMachine.EnemyAudioSource.PlayOneShot(stateMachine.RunningClip);
       
    }

    public override void Exit()
    {         
        stateMachine.Agent.isStopped = true;
        stateMachine.Agent.updateRotation = false;
        stateMachine.Agent.updatePosition = false;
    }
}
