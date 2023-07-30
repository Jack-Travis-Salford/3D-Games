using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    //State when enemy isn't doing anything
    private int _IdleHash = Animator.StringToHash("EnemyIdle");
    
    private const float CrossFadeDuration = 0.1f;
    
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(_IdleHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        _isGrounded = stateMachine.Controller.isGrounded;
        //Move player 0 by vector3.zero
        if (ShouldRun())
        {
            stateMachine.SwitchState(new EnemyChaseState(stateMachine));
            return;
        }
        stateMachine.SwitchState(new EnemyDefensiveState(stateMachine));
    }

    public override void Exit()
    {
    }
}
