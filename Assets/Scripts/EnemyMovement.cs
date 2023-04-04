using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [Range(-360, 360f)]
    [SerializeField]
    private float _detectAngle; // default = 315
    [SerializeField]
    private EnemyTypes _enemyMoveType;
    private Vector2 _moveDirection;

    // Ground enemies
    private Vector2 _detectVector; // the vector corresponding to the detect angle

    [Range(1, 10f)]
    [SerializeField]
    // Air enemies
    private float _timeUntilMoveDirFlip; // time until a flying enemy will flip its move direction
    private float _timeSinceLastMoveDirFlip;

    private enum EnemyTypes : int
    {
        Ground, Air
    }

    private new void Awake()
    {
        base.Awake();
        _detectVector = Game_Manager.GetVector2FromAngle(_detectAngle);
        _moveDirection = Vector2.right;

        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                RigidBody.gravityScale = 1;
                break;
            case EnemyTypes.Air:
                RigidBody.gravityScale = 0;
                break;
            default:
                RigidBody.gravityScale = 1;
                break;
        }
    }
    protected override void MoveHorizontally()
    {
        // Only consider horizontal movement, since enemies (for now) do not jump
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * _moveDirection, MoveAccel);

        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                // Did not detect a platform in front of it
                if (CheckIfAtEndOfPlatform())
                {
                    FlipMoveDirection();
                }
                break;
            case EnemyTypes.Air:
                if (Time.time >= _timeSinceLastMoveDirFlip + _timeUntilMoveDirFlip)
                {
                    _timeSinceLastMoveDirFlip = Time.time;
                    FlipMoveDirection();
                }
                break;
            default:
                break;
        }
    }
    protected override void MoveVertically()
    {
        throw new System.NotImplementedException();
    }
    private bool CheckIfAtEndOfPlatform()
    {
        Debug.DrawRay(transform.position, _detectVector, Color.white, Game_Manager.DebugRayLifeTime);
        // If its raycast detects the end of its current platform, switch directions (raycast detection angle will flip to match its direction as well)
        Ray ray = new Ray(transform.position, _detectVector);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, Game_Manager.PlatformMask);
        return !hit || !Game_Manager.IsObjectAPlatform(hit.collider.gameObject);
    }
    private void FlipMoveDirection()
    {
        _detectVector = Vector2.Reflect(_detectVector, Vector2.right);
        _detectAngle = Game_Manager.GetAngleFromVector2(_detectVector);
        _moveDirection *= -1;
    }
}
