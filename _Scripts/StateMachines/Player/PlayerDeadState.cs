using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        GameGlobalVals.Instance.SetGamePaused(true);
        GameHandler.Instance.PlayerIsDead();
    }

    public override void Tick(float deltaTime)
    {
        GameGlobalVals.Instance.SetGamePaused(true);
        
    }

    public override void Exit()
    {
    }
   
}
