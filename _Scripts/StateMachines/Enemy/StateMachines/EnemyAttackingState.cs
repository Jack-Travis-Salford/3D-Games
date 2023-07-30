using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private int _attackHash = Animator.StringToHash("EnemyAttack");
    private Vector3 distanceToPlayer;
    private const float CrossFadeDuration = 0.1f;
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.isStopped = false;
        stateMachine.Agent.updatePosition = true;
        stateMachine.Animator.CrossFadeInFixedTime(_attackHash, CrossFadeDuration);
        distanceToPlayer = Player.transform.position - stateMachine.transform.position;
    }

    public override void Tick(float deltaTime)
    {
        //Move enemy and face player whilst swinging
         distanceToPlayer = Player.transform.position - stateMachine.transform.position;
         Move(distanceToPlayer*0.9f, deltaTime);
         FacePlayer();
         PlayAudio();
        //Check to see if attack animation is finished
         AnimatorStateInfo currentAnimation = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
         AnimatorStateInfo nextAnimation = stateMachine.Animator.GetNextAnimatorStateInfo(0);
         if (!nextAnimation.IsTag("EnemyAttack") && currentAnimation.IsTag("EnemyAttack") && currentAnimation.normalizedTime >= 1f)
         { 
             stateMachine.SwitchState(new EnemyDefensiveState(stateMachine)); 
         }
       
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
        stateMachine.Agent.updatePosition = false;
        
    }
}
