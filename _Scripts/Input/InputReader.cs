using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

//Controls all inputs
public class InputReader : MonoBehaviour, Controls.IPlayerMovementActions
{ 
    [field: SerializeField] public GameObject MenuCanvas { get; private set; }
    [field: SerializeField] public GameObject Menu { get; private set; }
    [field: SerializeField] public GameObject MenuSettings { get; private set; }
    [field: SerializeField] public TextMeshProUGUI helpText { get; private set; }
   public Vector2 MovementValue { get; private set; }
    public Vector2 CamRotation { get; private set; }
    
    private Controls _controls;
    public event Action JumpEvent;
    public event Action StartRunEvent;
    public event Action EndRunEvent;
    public event Action StartBlockEvent;
    public event Action EndBlockEvent;
    public event Action AttackEvent;
    
    private void Start()
    {
        _controls = new Controls();
        _controls.PlayerMovement.SetCallbacks(this);
        _controls.PlayerMovement.Enable();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameGlobalVals.Instance.SetGameStarted(true);
            helpText.enabled = false;
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!MenuCanvas.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameGlobalVals.Instance.SetGamePaused(true);
                MenuCanvas.SetActive(true);
            }
            else if (MenuSettings.activeSelf)
            {
                MenuSettings.SetActive(false);
                Menu.SetActive(true);
            }else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                MenuCanvas.SetActive(false);
                GameGlobalVals.Instance.SetGamePaused(false);
            }
        }
       
       
    }

    private void OnDestroy()
    {
        _controls.PlayerMovement.Disable();
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartRunEvent?.Invoke();
        }

        if (context.canceled)
        {
            EndRunEvent?.Invoke();
        }
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackEvent?.Invoke();
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartBlockEvent?.Invoke();
        }

        if (context.canceled)
        {
            EndBlockEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        CamRotation = context.ReadValue<Vector2>();
    }
}
