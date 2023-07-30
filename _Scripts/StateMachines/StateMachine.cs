using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    //If the game was paused or not on the last Update() call
    private bool lastHandledGamePaused;

    //Handles game pausing/playing and calling state Update method (Tick)
    private void Update()
    {
        if (lastHandledGamePaused != GameGlobalVals.Instance.isGamePaused) 
        {
            currentState?.GamePaused(GameGlobalVals.Instance.isGamePaused);
            lastHandledGamePaused = GameGlobalVals.Instance.isGamePaused;
        }
        
        if (!GameGlobalVals.Instance.isGamePaused)
        {
            currentState?.Tick(Time.deltaTime); 
        }
        

    }
    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
