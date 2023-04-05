using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : EntityMovement
{
    private bool _isMoving;
    private PlayerControls _playerControls;

    private new void Awake()
    {
        base.Awake();
        _playerControls = new PlayerControls();
        transform.position = Game_Manager.LevelEntrance.transform.position;
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.started += _ => _isMoving = true;
        _playerControls.Movement.HorizontalMove.canceled += _ => _isMoving = false;
        _playerControls.Movement.VerticalMove.performed += _ => MoveVertically();
        _playerControls.Abilities.Attack.performed += _ => AbilityAttack();
        _playerControls.Abilities.Interact.performed += _ => AbilityInteract();
        _playerControls.Camera.ZoomOut.performed += _ => CameraMovement.SwitchCameraZoom();
    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            transform.position = Game_Manager.LevelEntrance.transform.position;
        }
        else if (Game_Manager.IsObjectAnEnemy(collision.gameObject))
        {
            EffectKnockback.SetNewTimeEffectApply();
            // Push the player away from the enemy

            // Find distance from the collided collider
            Vector2 distanceFromEnemy = collision.collider.transform.position - transform.position;
            // Get direction away from the collided collider, then normalize it
            distanceFromEnemy = (-1 * distanceFromEnemy).normalized;

            // Reset velocity and add force away from the collider collided with
            RigidBody.velocity = Vector2.zero;
            RigidBody.AddForce(new Vector2(EffectKnockback.ForceEffect.x * distanceFromEnemy.x, EffectKnockback.ForceEffect.y * distanceFromEnemy.y));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{name}: {collision.name}");
    }
    protected override void MoveHorizontally()
    {
        float MoveDirection = _playerControls.Movement.HorizontalMove.ReadValue<float>();
        // If not currently being knocked back (do not want to set velocity while being knocked back)
        if (EntityEffect.HasEnoughTimeHasPassed(EffectKnockback))
        {
            if (_isMoving)
            {
                // Lerp speed from current speed to the target horizontal speed + current vertical speed
                // Allows vertical speed to decrease by gravity instead of being reset
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, RigidBody.velocity * Vector2.up + MoveForce * MoveDirection * Vector2.right, MoveAccel);
            }
            else if (!_isMoving)
            {
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, RigidBody.velocity * Vector2.up, MoveDecel);
            }
        }
    }
    protected override void MoveVertically()
    {
        if (IsGrounded && EntityEffect.HasEnoughTimeHasPassed(EffectJump))
        {
            EffectJump.SetNewTimeEffectApply();
            RigidBody.AddForce(EffectJump.ForceEffect);
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
