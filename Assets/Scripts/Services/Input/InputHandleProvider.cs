using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarioECS
{
    public sealed class InputHandleProvider
    {
        public bool IsCurrentDeviceMouse => Gamepad.current == null;
        public Vector2 Direction {get; private set;}        
        public Vector2 LookDirection { get; private set; }
        public bool IsJump {get; private set;}
        public bool IsMoved {get; private set;}
        public bool IsRunning {get; private set;}
        public bool IsAttack { get; private set; }

        private readonly PlayerInputAction _control;

        private bool _isActive;


        public InputHandleProvider(PlayerInputAction control)
        {
            _control = control;
        }
        

        public void Enable()
        {
            if (_isActive) return;

            _control.Enable();

            _control.Player.Movement.performed += SetDirection;
            _control.Player.Movement.canceled += ReleaseDirection;
            _control.Player.Jump.started += ActiveJump;
            _control.Player.Jump.canceled += ActiveJump;
            _control.Player.Run.started += ActiveRunning;
            _control.Player.Run.canceled += ActiveRunning;
            _control.Player.CameraLook.performed += SetLook;
            _control.Player.CameraLook.canceled += SetLook;
            _control.Player.Attack.started += ActiveAttack;

            _isActive = true;
        }


        public void Disable()
        {
            if (!_isActive) return;

            _control.Player.Movement.performed -= SetDirection;
            _control.Player.Movement.canceled -= ReleaseDirection;
            _control.Player.Jump.started -= ActiveJump;
            _control.Player.Jump.canceled -= ActiveJump;
            _control.Player.Run.started -= ActiveRunning;
            _control.Player.Run.canceled -= ActiveRunning;
            _control.Player.CameraLook.performed -= SetLook;
            _control.Player.CameraLook.canceled -= SetLook;
            _control.Player.Attack.started -= ActiveAttack;

            _control.Disable();

            _isActive = false;
        }


        private void ActiveAttack(InputAction.CallbackContext ctx) => IsAttack = ctx.ReadValueAsButton();


        private void SetLook(InputAction.CallbackContext ctx) => LookDirection = ctx.ReadValue<Vector2>();


        private void ActiveJump(InputAction.CallbackContext ctx) => IsJump = ctx.ReadValueAsButton();


        private void ActiveRunning(InputAction.CallbackContext ctx) => IsRunning = ctx.ReadValueAsButton();
        

        private void SetDirection(InputAction.CallbackContext ctx) 
        {
            Direction = ctx.ReadValue<Vector2>();
            IsMoved = true;
        }


        private void ReleaseDirection(InputAction.CallbackContext ctx)
        {
            Direction = ctx.ReadValue<Vector2>();
            IsMoved = false;
        }

        
        public void ResetInput()
        {
            IsMoved = IsRunning = IsJump = IsAttack = false;
        }
    }
}