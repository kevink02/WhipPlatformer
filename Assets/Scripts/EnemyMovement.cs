using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [SerializeField]
    private EnemyTypes _enemyMoveType;
    private Vector2 _moveDirection;

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
        DetectVector = Game_Manager.GetVector2FromAngle(DetectAngle);
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
                if (!RayCastHit || !Game_Manager.IsObjectAPlatform(RayCastHit.collider.gameObject))
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
    private void FlipMoveDirection()
    {
        DetectVector = Vector2.Reflect(DetectVector, Vector2.right);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        _moveDirection *= -1;
    }
}
