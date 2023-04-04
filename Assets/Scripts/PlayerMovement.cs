using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : EntityMovement
{
    private bool _isGrounded;
    private bool _isMoving;
    private PlayerControls _playerControls;

    private new void Awake()
    {
        base.Awake();
        _isGrounded = true;
        _playerControls = new PlayerControls();
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.started += _ => _isMoving = true;
        _playerControls.Movement.HorizontalMove.canceled += _ => _isMoving = false;
        _playerControls.Movement.VerticalMove.performed += _ => MoveVertically();
        _playerControls.Abilities.Attack.performed += _ => AbilityAttack();
        _playerControls.Abilities.Interact.performed += _ => AbilityInteract();
    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAPlatform(collision.collider.gameObject))
        {
            _isGrounded = true;
        }
    }
    protected override void MoveHorizontally()
    {
        float moveDirection = _playerControls.Movement.HorizontalMove.ReadValue<float>();
        if (_isMoving)
        {
            // Lerp speed from current speed to the target horizontal speed + current vertical speed
            // Allows vertical speed to decrease by gravity instead of being reset
            RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, RigidBody.velocity * Vector2.up + MoveForce * moveDirection * Vector2.right, MoveAccel);
        }
        else
        {
            RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, RigidBody.velocity * Vector2.up, MoveDecel);
        }
    }
    protected override void MoveVertically()
    {
        if (_isGrounded)
        {
            _isGrounded = false;
            RigidBody.AddForce(JumpForce * Vector2.up);
        }
    }
    private void AbilityAttack()
    {
        Debug.Log($"{name}: Attacking");
    }
    private void AbilityInteract()
    {
        Debug.Log($"{name}: Interacting");
    }
}
