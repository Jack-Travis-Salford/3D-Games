using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public abstract class EnemyBaseState : State
{
    //Handle state switching (IsInChaseRange, Move, FacePlayer) 
    protected EnemyStateMachine stateMachine;
    protected GameObject Player;
    
    
    protected bool _isGrounded = true;
    protected float _gravity = -15f;
    protected float _verticalVelocity = 0f;
    protected float sqrDistFromPlayer => (Player.transform.position - stateMachine.transform.position).sqrMagnitude;
    
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        Player = GameObject.FindGameObjectWithTag("Player");
        _isGrounded = stateMachine.Controller.isGrounded;
    }
    protected bool ShouldRetreat()
    {
        return sqrDistFromPlayer <= stateMachine.SqrStartRetreatDistance;
    }

    protected bool ShouldContinueChase()
    {
        return sqrDistFromPlayer >= stateMachine.SqrEndRunDistance;
    }

    protected bool ShouldExitRetreat()
    {
        return sqrDistFromPlayer >= stateMachine.SqrTargetDistFromPlayer;
    }

    protected bool ShouldRun()
    {
        return sqrDistFromPlayer >= stateMachine.SqrStartRunDistance;
    }
    protected bool ShouldChase()
    {
        return sqrDistFromPlayer >= stateMachine.SqrTargetDistFromPlayer;
    }
     
    protected int WhichDefenceMovement()
    {
        //-1 - Move away
        //0 - Dont move
        //1 - Move toward enemy
        if(ShouldRetreat())
        {
            return -1;
        }else if (ShouldChase())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    
    protected float ApplyGravity(float deltaTime)
    {
        _verticalVelocity += _gravity * deltaTime;
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
        return _verticalVelocity;
    }
    
    protected void Move(Vector3 movement, float deltaTime)
    {
        movement.y += ApplyGravity(deltaTime);
        stateMachine.Controller.Move(movement * deltaTime);
        stateMachine.Ragdoll.transform.position = stateMachine.transform.position;
    }

    /*
     * Replaces nav mesh agent rotation when close to player.
     * When the enemy is close to the player, the enemy should always face them.
     * Nav Mesh will face the direction of travel
     * Hence, this method is used instead for the rotation of the enemy
     */
    protected void FacePlayer()
    {
        Vector3 lookPos = Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        
    }
    public override void GamePaused(bool isPaused)
    {
        stateMachine.Agent.isStopped = isPaused;
        stateMachine.Animator.enabled = !isPaused;
        stateMachine.Agent.updatePosition = !isPaused;
    }
    
    
}
