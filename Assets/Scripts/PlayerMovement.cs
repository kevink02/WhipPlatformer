using System.Collections;
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
    [SerializeField]
    private Vector2 _spawnPoint;

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
        if (collision.collider.CompareTag("VoidCheck"))
        {
            transform.position = _spawnPoint;
        }
        else if (collision.collider.CompareTag("Enemy") && Game_Manager.CheckIfEnoughTimeHasPassed(_timeOfLastKnockBack, _knockBackEffectTime))
        {
            _isOnKnockBack = true;
            _timeOfLastKnockBack = Time.time;
            // Push the player away from the enemy

            // Find distance from the collided collider
            Vector2 distanceFromEnemy = collision.collider.transform.position - transform.position;
            // Get direction away from the collided collider, then normalize it
            distanceFromEnemy = (-1 * distanceFromEnemy).normalized;

            // Reset velocity and add force away from the collider collided with
            RigidBody.velocity = Vector2.zero;
            RigidBody.AddForce(new Vector2(_knockBackForce.x * distanceFromEnemy.x, _knockBackForce.y * distanceFromEnemy.y));
        }
        else if (Game_Manager.CheckIfEnoughTimeHasPassed(_timeOfLastKnockBack, _knockBackEffectTime))
        {
            _isOnKnockBack = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAPlatform(collision.collider.gameObject) && HasCollidedWithAPlatformAtDetectAngle() && Game_Manager.CheckIfEnoughTimeHasPassed(_timeOfLastJump, _jumpCooldownTime))
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
