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
    private PlayerHealth _playerHealth;

    private new void Awake()
    {
        base.Awake();
        _playerAttack = GetComponentInChildren<PlayerAttack>();
        _playerControls = new PlayerControls();
        _playerHealth = GetComponent<PlayerHealth>();
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
        Game_Manager.GameEnd += _playerControls.Disable;
    }
    private void OnDisable()
    {
        _playerControls.Disable();
        Game_Manager.GameEnd -= _playerControls.Disable;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            SetPositionToSpawnPoint();
        }
        else if (Game_Manager.IsObjectAnEnemy(collision.gameObject) && EntityEffect.HasEnoughTimePassed(EffectJump))
        {
            _playerHealth.TakeDamage();

            // Health gets reset when calling TakeDamage function, so check if current Health = MaxHealth, not = 0
            if (_playerHealth.Health == _playerHealth.MaxHealth)
            {
                // Prevent knockback effect from occurring when player respawns
                RigidBody.velocity = Vector2.zero;
                return;
            }

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
        if (PauseMenu.IsPaused())
            return;

        // Either -1 or 1
        float moveDirection = _playerControls.Movement.HorizontalMove.ReadValue<float>();
        UpdateSpriteDirection(moveDirection);

        // If not currently being knocked back (do not want to set velocity while being knocked back)
        if (EntityEffect.HasEnoughTimePassed(EffectKnockback))
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
    protected override void UpdateColliderSize()
    {
        // Collider shouldn't be too big since it will interfere with the platforms in tilemaps
        // Divide by 100 since that is the sprite's pixels per unit value?
        // Divide more from x since it seems collider for player is too big (prevents them from falling down holes if walking continuously)
        Collider.size = new Vector2(ComponentSprite.sprite.rect.size.x / 135, ComponentSprite.sprite.rect.size.y / 100);
    }
    private void DoJump()
    {
        if (PauseMenu.IsPaused())
            return;

        if (IsGrounded && EntityEffect.HasEnoughTimePassed(EffectJump))
        {
            EffectJump.SetNewTimeEffectApply();
            RigidBody.AddForce(EffectJump.ForceEffect);
        }
    }
    private void AbilityAttack()
    {
        if (PauseMenu.IsPaused())
            return;

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
        if (PauseMenu.IsPaused())
            return;

        if (_isAtExit)
        {
            GameObject endFlag = GameObject.FindGameObjectWithTag("LevelEnd");
            if (!endFlag)
            {
                throw new System.Exception("There is no end flag object");
            }
            Animator endFlagAnimator = endFlag.GetComponent<Animator>();
            endFlagAnimator.SetBool("HasGameEnded", true);
            Game_Manager.GetSingleton().WinGame(this);
        }
        else if (_isAtCheckpoint)
        {
            Game_Manager.GetSingleton().SetLevelProgressText("Checkpoint set!", transform.position);
            SpawnPoint = _checkpointObject.transform;
        }
    }
}
