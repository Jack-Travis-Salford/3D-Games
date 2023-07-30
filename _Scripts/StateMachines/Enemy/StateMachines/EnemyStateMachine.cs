using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyStateMachine : StateMachine
{
    //State machine for all enemy states
    
    //Vars to be set in Unity
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    //How close the enemy is to the player before retreating
    [field: SerializeField] public float SqrStartRetreatDistance { get; private set; }
    //When enemy should begin running to catch up
    [field: SerializeField] public float SqrStartRunDistance { get; private set; }
     
    //How close to the play the enemy aim for
    [field: SerializeField] public float SqrTargetDistFromPlayer { get; private set; }
    [field: SerializeField] public float SqrEndRunDistance { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public GameObject ShieldCollider { get; private set; }
    [field: SerializeField] public AudioSource EnemyAudioSource { get; private set; }
    [field: SerializeField] public AudioClip WalkingClip { get; private set; }
    [field: SerializeField] public AudioClip RunningClip { get; private set; }
    [field: SerializeField] public GameObject EnemyRig { get; private set; }
    public EnemyBrain EnemyBrain;
    [field: SerializeField] public SkinnedMeshRenderer Render { get; private set; }
    
    private void Start()
    {
        
        //Disable navmesh agent functions until user is in correct states
        Agent.isStopped = true;
        Agent.updatePosition = false; //Prevents NavMeshAgent from controlling enemy
        Agent.updateRotation = false;
        SwitchState(new EnemyIdleState(this));
        Weapon.SetAttack(GameGlobalVals.Instance.enemyWeaponDamage);
        
        switch (Random.Range(0, 3))
        {
            case 0:
                EnemyBrain = new RandomBrain(this);
                break;
            case 1:
                EnemyBrain = new Hybrid(this);
                break;
            case 2:
                EnemyBrain = new Aggressive(this);
                break;
            default:
                EnemyBrain = new Aggressive(this);
                break;
        }
    }
    
    
    private void OnEnable()
    {
       Health.onDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.onDie -= HandleDie;
    }
    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

}
