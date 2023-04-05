﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : EntityMovement
{
    [Range(0.01f, 1f)]
    [SerializeField]
    private float _knockBackEffectTime; // default = 0.5f
    [Range(0.01f, 1f)]
    [SerializeField]
    private float _jumpCooldownTime; // default = 0.5f
    [SerializeField]
    private Vector2 _knockBackForce; // default = 1000f, 1f

    private bool _isGrounded;
    private bool _isMoving;
    private bool _isOnKnockBack;
    private float _timeOfLastKnockBack;
    private float _timeOfLastJump;
    private PlayerControls _playerControls;

    private new void Awake()
    {
        base.Awake();
        _isGrounded = true;
        _playerControls = new PlayerControls();

        // Verify values of the knockback force
        if (_knockBackForce.x < 1)
            _knockBackForce.x = 750f;
        if (_knockBackForce.y <= 0)
            _knockBackForce.y = 100f;
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
        if (collision.collider.CompareTag("Enemy") && Time.time >= _timeOfLastKnockBack + _knockBackEffectTime)
        {
            _isOnKnockBack = true;
            _timeOfLastKnockBack = Time.time;
            // Push the player away from the enemy

            // Reset velocity and add force away from the point of collision
            float distanceFromEnemyX = -1 * (collision.collider.transform.position.x - transform.position.x);
            float distanceFromEnemyY = -1 * (collision.collider.transform.position.y - transform.position.y);
            Vector2 tempVelocity = new Vector2(distanceFromEnemyX, distanceFromEnemyY).normalized;
            //Vector2 tempVelocity = -1 * RigidBody.velocity.normalized;

            RigidBody.velocity = Vector2.zero;
            RigidBody.AddForce(new Vector2(_knockBackForce.x * tempVelocity.x, _knockBackForce.y * tempVelocity.y));
        }
        else if (Time.time >= _timeOfLastKnockBack + _knockBackEffectTime)
        {
            _isOnKnockBack = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAPlatform(collision.collider.gameObject) && HasCollidedWithAPlatformAtDetectAngle() && Time.time >= _timeOfLastJump + _jumpCooldownTime)
        {
            _isGrounded = true;
            _timeOfLastJump = Time.time;
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
        // Otherwise if not moving and currently being knocked back, do not want to set velocity as knockback velocity is in effect
        else if (!_isMoving && !_isOnKnockBack)
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
