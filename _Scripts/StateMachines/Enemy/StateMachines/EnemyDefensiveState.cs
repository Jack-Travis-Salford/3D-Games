using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefensiveState : EnemyBaseState
{
    private int _blockBlendTreeHash = Animator.StringToHash("EnemyBlockBlendTree");
    private int _enemyMovementSpeedHash = Animator.StringToHash("EnemyMovementSpeed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public EnemyDefensiveState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.isStopped = false;
        stateMachine.Agent.updatePosition = true;
        stateMachine.Animator.CrossFadeInFixedTime(_blockBlendTreeHash, CrossFadeDuration);
        stateMachine.ShieldCollider.SetActive(true);
        
    }

    public override void Tick(float deltaTime)
    {
        
        _isGrounded = stateMachine.Controller.isGrounded;
        
        if (ShouldRun())
        {
            stateMachine.SwitchState(new EnemyChaseState(stateMachine));
            return;
        }       
        FacePlayer();
 
        Vector3 movement;
        switch (WhichDefenceMovement())
        {
            case -1:
                movement = Retreat(deltaTime);
                stateMachine.Animator.SetFloat(_enemyMovementSpeedHash, 0.25f, AnimatorDampTime, deltaTime);
                PlayAudio();
                break;
            
            case 1:
                movement = Advance(deltaTime);
                stateMachine.Animator.SetFloat(_enemyMovementSpeedHash, 0.75f, AnimatorDampTime, deltaTime);
                PlayAudio();
                break;
            case 0:default:
                movement = Vector3.zero;
                stateMachine.Animator.SetFloat(_enemyMovementSpeedHash, 0.50f, AnimatorDampTime, deltaTime);
                //CHANGE AFTER
                if (stateMachine.EnemyBrain.ShouldAttack())
                {
                    stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
                }
                break;
        }
        movement.y = 0f;
        Move(movement,deltaTime);
        
    }

    public Vector3 Advance(float deltaTime)
    {
        stateMachine.Agent.destination = Player.transform.position;
        return stateMachine.Agent.desiredVelocity.normalized * (stateMachine.MovementSpeed);
    }

    public Vector3 Retreat(float deltaTime)
    {
        float sqrDistFromPlayer = (Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        //Calculate when the enemy needs to move to, so that they are the target distance away.
        stateMachine.Agent.destination = stateMachine.Agent.transform.position + ((9 / sqrDistFromPlayer) * (stateMachine.Agent.transform.position - Player.transform.position));
        return stateMachine.Agent.desiredVelocity.normalized * (stateMachine.MovementSpeed*0.75f);
    }
    private void PlayAudio()
    {
        if (stateMachine.EnemyAudioSource.isPlaying)
        {
            return;
        }
        stateMachine.EnemyAudioSource.PlayOneShot(stateMachine.WalkingClip);
       
    }

    public override void Exit()
    {
        stateMachine.Agent.isStopped = true;
        stateMachine.Agent.updatePosition = false;
        stateMachine.ShieldCollider.SetActive(false);
    }
}
