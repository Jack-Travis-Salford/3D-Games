using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float JumpingForce { get; private set; }
    [field: SerializeField] public Camera Cam { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public GameObject ShieldCollider { get; private set; }
     //Holds hash of animations that are playing/what to change to
    private int currentAnimation;
 
    //Animation related stuff
    private readonly int _moveForwardSpeedHash = Animator.StringToHash("MovementForwardSpeed");
    private readonly int _moveRightSpeedHash = Animator.StringToHash("MovementRightSpeed");
    private readonly int _movementBlendTreeHash = Animator.StringToHash("PlayerWalkingBlendTree");
    private readonly int _jumpHash = Animator.StringToHash("jump");
    private readonly int _attackHash = Animator.StringToHash("AttackingBlendTree");
    private readonly int _blockHash = Animator.StringToHash("BlockingBlendTree");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    

    //For camera rotation
    private float _xRotation = 0f;
    private float _xSensitivity = 15f;
    private float _ySensitivity = 15f;
    //For _gravity
    private bool _isGrounded = true;
    private bool _isAttacking;
    private bool _isBlocking;
    private float _gravity = -15f;
    private float _verticalVelocity = 0f;

    private bool _isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        InputReader.JumpEvent += OnJump;
        InputReader.StartRunEvent += OnRun;
        InputReader.EndRunEvent += OnCancelRun;
        InputReader.AttackEvent += OnAttack;
        InputReader.StartBlockEvent += OnBlock;
        InputReader.EndBlockEvent += OnCancelBlock;
        Animator.CrossFadeInFixedTime(_movementBlendTreeHash, CrossFadeDuration);
        currentAnimation = _movementBlendTreeHash;
        _isGrounded = Controller.isGrounded;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Weapon.SetAttack(20);
    }
    // Update is called once per frame
    void Update()
    {
        _isGrounded = Controller.isGrounded;
        MovePlayer(Time.deltaTime);
        UpdateAnimator(Time.deltaTime);
        RotateCamera(Time.deltaTime);
    }
    private void MovePlayer(float deltaTime)
    {
        Vector3 movement = Vector3.zero;
        float speedMultiplier = 1f;
        if (_isRunning && !_isAttacking && _isGrounded && currentAnimation != _blockHash )
        {
            speedMultiplier = 1.5f;
        }
        //CHANGE
        if (_isBlocking)
        {
            speedMultiplier *= 0.5f;
        }
        movement.x = InputReader.MovementValue.x * MovementSpeed *speedMultiplier;
        movement.z = InputReader.MovementValue.y* MovementSpeed *speedMultiplier;
        _verticalVelocity += _gravity * deltaTime;
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
        movement.y = _verticalVelocity;
        Controller.Move( transform.TransformDirection(movement) * deltaTime);
    }
    private void RotateCamera(float deltaTime)
    {
        float mouseX = InputReader.CamRotation.x;
        float mouseY = InputReader.CamRotation.y;
        _xRotation -= (mouseY * Time.deltaTime) * _ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -70f, 70f);
        Cam.transform.localRotation = Quaternion.Euler(_xRotation,0,0);
        transform.Rotate(Vector3.up * ((mouseX* deltaTime)*_xSensitivity));
    }
    
    private void UpdateAnimator(float deltaTime)
    {
        //Change from jump to walk when returning to the ground
        AnimatorStateInfo currentAnimationInfo = Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimationInfo = Animator.GetNextAnimatorStateInfo(0);
        if (_isAttacking)
        {
            //if animation is playing and finished
            if (currentAnimationInfo.IsTag("attacking") && currentAnimationInfo.normalizedTime >= 0.9)
            {
                _isAttacking = false;
            }
        }

        if (!_isAttacking && !_isGrounded && currentAnimation != _jumpHash)
        {
            Animator.CrossFadeInFixedTime(_jumpHash, CrossFadeDuration*3);
            ShieldCollider.SetActive(false);
            currentAnimation = _jumpHash;
        }
        if (_isBlocking && !_isAttacking && _isGrounded && currentAnimation != _blockHash)
        {
            Animator.CrossFadeInFixedTime(_blockHash,CrossFadeDuration);
            ShieldCollider.SetActive(true);
            currentAnimation = _blockHash;
        }
        if (_isGrounded && !_isBlocking && !_isAttacking && currentAnimation != _movementBlendTreeHash)
        {
            Animator.CrossFadeInFixedTime(_movementBlendTreeHash,CrossFadeDuration);
            ShieldCollider.SetActive(false);
            currentAnimation = _movementBlendTreeHash;
        }
        CalcMovementBlendTreeVals(deltaTime);
      }
    
    private void CalcMovementBlendTreeVals(float deltaTime)
    {
        float movementY = 0f;
        float movementX = 0f;
        switch (InputReader.MovementValue.y)
        {
            case (>0f):
                if (_isRunning || _isAttacking || currentAnimation == _blockHash )
                {
                    movementY = 1f; 
                }
                else
                {
                    movementY = 0.5f;
                }
                    
                break;
            case (< 0f):
                if (_isRunning || _isAttacking || currentAnimation == _blockHash)
                {
                    movementY = -1f; 
                }
                else
                {
                    movementY = -0.5f;
                }
                break;
            default:
                movementY = 0f;
                break;
        }
        Animator.SetFloat(_moveForwardSpeedHash, movementY, AnimatorDampTime, deltaTime);
        switch (InputReader.MovementValue.x)
        {
            case (> 0f):
                movementX = 1f;
                break;
            case (< 0f):
                movementX = -1f;
                break;
            default:
                movementX = 0f;
                break;
        }
        Animator.SetFloat(_moveRightSpeedHash, movementX, AnimatorDampTime, deltaTime);
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
    private void OnJump()
    {
        if (_isGrounded)
        {
            _verticalVelocity += JumpingForce;
            if (!_isAttacking)
            {
                Animator.CrossFadeInFixedTime(_jumpHash,CrossFadeDuration);
                ShieldCollider.SetActive(false);
                currentAnimation = _jumpHash;
            }
        }
    }

    private void OnAttack()
    {
        if (!_isAttacking)
        {
            Animator.CrossFadeInFixedTime(_attackHash, CrossFadeDuration);
            _isAttacking = true;
            currentAnimation = _attackHash;
        }
    }
    public void OnDestroy()
    {
        InputReader.JumpEvent -= OnJump;
        InputReader.StartRunEvent -= OnRun;
        InputReader.EndRunEvent -= OnCancelRun;
        InputReader.AttackEvent -= OnAttack;
        InputReader.StartBlockEvent -= OnBlock;
        InputReader.EndBlockEvent -= OnCancelBlock;
    }

}
