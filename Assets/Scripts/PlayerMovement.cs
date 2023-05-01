using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : EntityMovement
{
    [SerializeField]
    private AnimationClip _animationAttack;
    private bool _isMoving;
    private bool _isAtCheckpoint;
    private bool _isAtExit;
    private GameObject _checkpointObject;
    private PlayerAttack _playerAttack;
    private PlayerControls _playerControls;

    private new void Awake()
    {
        base.Awake();
        _playerAttack = GetComponentInChildren<PlayerAttack>();
        _playerControls = new PlayerControls();
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.started += _ => _isMoving = true;
        _playerControls.Movement.HorizontalMove.canceled += _ => _isMoving = false;
        _playerControls.Movement.VerticalMove.performed += _ => DoJump();
        _playerControls.Abilities.Attack.performed += _ => AbilityAttack();
        _playerControls.Abilities.Interact.performed += _ => AbilityInteract();
        _playerControls.UI.CameraZoom.performed += _ => CameraMovement.SwitchCameraZoom();
        _playerControls.UI.PauseMenu.performed += _ => PauseMenu.TogglePause();
    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            SetPositionToSpawnPoint();
        }
        else if (Game_Manager.IsObjectAnEnemy(collision.gameObject) && EntityEffect.HasEnoughTimeHasPassed(EffectJump))
        {
            EffectKnockback.SetNewTimeEffectApply();
            // Push the player away from the enemy

            // Find distance from the collided collider
            Vector2 distanceFromEnemy = collision.collider.transform.position - transform.position;
            // Only account for the horizontal portion of knockback
            distanceFromEnemy = new Vector2(Mathf.Sign(distanceFromEnemy.x), 0);
            // Get direction away from the collided collider, then normalize it
            distanceFromEnemy = (-1 * distanceFromEnemy).normalized;

            // Reset velocity and add force away from the collider collided with
            RigidBody.velocity = Vector2.zero;
            RigidBody.AddForce(new Vector2(EffectKnockback.ForceEffect.x * distanceFromEnemy.x, EffectKnockback.ForceEffect.y * distanceFromEnemy.y));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Game_Manager.IsObjectALevelEnd(collision.gameObject))
            _isAtExit = true;
        else if (Game_Manager.IsObjectALevelCheckpoint(collision.gameObject))
        {
            _isAtCheckpoint = true;
            _checkpointObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Game_Manager.IsObjectALevelEnd(collision.gameObject))
            _isAtExit = false;
        else if (Game_Manager.IsObjectALevelCheckpoint(collision.gameObject))
        {
            _isAtCheckpoint = false;
            _checkpointObject = null;
        }
    }
    protected override void DoMovement()
    {
        // Either -1 or 1
        float moveDirection = _playerControls.Movement.HorizontalMove.ReadValue<float>();
        UpdateSpriteDirection(moveDirection);

        // If not currently being knocked back (do not want to set velocity while being knocked back)
        if (EntityEffect.HasEnoughTimeHasPassed(EffectKnockback))
        {
            if (_isMoving)
            {
                // Lerp speed from current speed to the target horizontal speed + current vertical speed
                // Allows vertical speed to decrease by gravity instead of being reset
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, RigidBody.velocity * Vector2.up + MoveForce * moveDirection * Vector2.right, MoveAccel);
            }
            else if (!_isMoving)
            {
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, RigidBody.velocity * Vector2.up, MoveDecel);
            }
            ComponentAnimator.SetBool("IsWalking", _isMoving && IsGrounded);
        }
    }
    protected override void UpdateSpriteDirection(float moveDirection)
    {
        // Change scale instead of flipping sprites so that player's child object for attack animation can update position if player turns
        if (moveDirection == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);

            //ComponentSprite.flipX = false;
            //_playerAttack.ComponentSprite.flipX = false;
        }
        else if (moveDirection == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);

            //ComponentSprite.flipX = true;
            //_playerAttack.ComponentSprite.flipX = true;
        }
    }
    private void DoJump()
    {
        if (IsGrounded && EntityEffect.HasEnoughTimeHasPassed(EffectJump))
        {
            EffectJump.SetNewTimeEffectApply();
            RigidBody.AddForce(EffectJump.ForceEffect);
        }
    }
    private void AbilityAttack()
    {
        // Prevent animation glitches by spamming attack triggers
        // If the attack animation (on its animation (child) object) is playing, skip rest of function
        if (!ComponentSprite.enabled)
        {
            return;
        }

        _playerAttack.gameObject.SetActive(true);
        ComponentAnimator.enabled = false;
        ComponentSprite.enabled = false;
        _playerAttack.AbilityAttack();
        StartCoroutine(EndAnimationAttack());
    }
    private IEnumerator EndAnimationAttack()
    {
        yield return new WaitForSeconds(PlayerAttack.AnimationAttackDuration);
        ComponentAnimator.enabled = true;
        ComponentSprite.enabled = true;
    }
    private void AbilityInteract()
    {
        Debug.Log($"{name}: Interacting");
        if (_isAtExit)
            Debug.Log($"{name}: I won!");
        else if (_isAtCheckpoint)
        {
            Debug.Log($"{name}: I reset my spawn point");
            SpawnPoint = _checkpointObject.transform;
        }
    }
}
