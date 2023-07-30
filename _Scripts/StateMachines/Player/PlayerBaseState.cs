using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;
    //For camera rotation
    

    //For _gravity

    private float _gravity = -15f;
    protected const float AnimatorDampTime = 0.1f;
    protected const float CrossFadeDuration = 0.1f;
   
    
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        stateMachine._isGrounded = stateMachine.Controller.isGrounded;
    }
    protected void RotateCamera(float deltaTime)
    {
        float mouseX = stateMachine.InputReader.CamRotation.x;
        float mouseY = stateMachine.InputReader.CamRotation.y;
        stateMachine._xRotation -= (mouseY * Time.deltaTime) * Settings.Instance.cameraSensitivity;
        stateMachine._xRotation = Mathf.Clamp(stateMachine._xRotation, -70f, 70f);
        stateMachine.Cam.transform.localRotation = Quaternion.Euler(stateMachine._xRotation,0,0);
        stateMachine.transform.Rotate(Vector3.up * ((mouseX* deltaTime)*Settings.Instance.cameraSensitivity));
    }
    
    protected void MovePlayer(float deltaTime, float speedMultiplier)
    {
        Vector3 movement = Vector3.zero;
      
        movement.x = stateMachine.InputReader.MovementValue.x * stateMachine.MovementSpeed *speedMultiplier;
        movement.z = stateMachine.InputReader.MovementValue.y* stateMachine.MovementSpeed *speedMultiplier;
        stateMachine._verticalVelocity += _gravity * deltaTime;
        if (stateMachine._isGrounded && stateMachine._verticalVelocity < 0)
        {
            stateMachine._verticalVelocity = -2f;
        }
        movement.y = stateMachine._verticalVelocity;
        stateMachine.Controller.Move( stateMachine.transform.TransformDirection(movement) * deltaTime);
    }
    public override void GamePaused(bool isPaused)
    {
        stateMachine.Animator.enabled = !isPaused;
    }
    
}
