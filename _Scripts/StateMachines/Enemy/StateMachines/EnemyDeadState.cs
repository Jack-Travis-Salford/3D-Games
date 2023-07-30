using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private int _deadHash = Animator.StringToHash("EnemyDead");
    private const float CrossFadeDuration = 0.1f;
    private float timePassed = 0f;
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Weapon.gameObject.SetActive(false);
        stateMachine.Animator.CrossFadeInFixedTime(_deadHash, CrossFadeDuration);
        stateMachine.Agent.updatePosition = false;
        GameHandler.Instance.ReportEnemyDead(stateMachine);
        //GameHandler.Instance.ReportEnemyDead(stateMachine);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Agent.updatePosition = false;
        if (!stateMachine.Animator.enabled)
        {
            return;
        }

        timePassed += deltaTime;
        if (timePassed > 5f)
        {
            stateMachine.Animator.enabled = false;
            stateMachine.Controller.enabled = false;
            
        }
    }

    public override void Exit()
    {
    }
}
