using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float JumpingForce { get; private set; }
    [field: SerializeField] public Camera Cam { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public GameObject ShieldCollider { get; private set; }
    [field: SerializeField] public float runningSpeedMultiplier { get; private set; }
    [field: SerializeField] public float blockingSpeedMultiplier { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public AudioSource playerAudioSource { get; private set; }
    [field: SerializeField] public AudioSource SwordAudioSource { get; private set; }
    [field: SerializeField] public AudioClip playerWalking { get; private set; }
    [field: SerializeField] public AudioClip playerRunning { get; private set; }
 
    public bool _isBlocking;
    public bool _isRunning = false;
    public float _verticalVelocity = 0f;
    public bool _isGrounded = true;
    public float _xRotation = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Weapon.SetAttack(20);
        SwitchState(new PlayerMovementState(this));
        GameGlobalVals.Instance.isGamePaused = false;
    }

    private void OnEnable()
    {
        InputReader.StartRunEvent += OnRun;
        InputReader.EndRunEvent += OnCancelRun;
        InputReader.StartBlockEvent += OnBlock;
        InputReader.EndBlockEvent += OnCancelBlock;
        Health.onDie += HandleDie;
    }

    private void OnDestroy()
    {
        InputReader.StartRunEvent -= OnRun;
        InputReader.EndRunEvent -= OnCancelRun;
        InputReader.StartBlockEvent -= OnBlock;
        InputReader.EndBlockEvent -= OnCancelBlock;
        Health.onDie -= HandleDie;
    }
    private void OnRun()
    {
        _isRunning = true;
    }
    
    private void OnCancelRun()
    {
        _isRunning = false;
    }

    private void OnBlock()
    {
        _isBlocking = true;
    }
    private void OnCancelBlock()
    {
        _isBlocking = false;
    }
    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }
   
}
