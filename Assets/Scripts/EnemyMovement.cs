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
        Ground, AirVertical, AirHorizontal
    }

    private new void Awake()
    {
        base.Awake();
        DetectVector = Game_Manager.GetVector2FromAngle(DetectAngle);

        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                _moveDirection = Vector2.right;
                RigidBody.gravityScale = 1;
                break;
            case EnemyTypes.AirVertical:
                _moveDirection = Vector2.up;
                RigidBody.gravityScale = 0;
                break;
            case EnemyTypes.AirHorizontal:
                _moveDirection = Vector2.right;
                RigidBody.gravityScale = 0;
                break;
            default:
                _moveDirection = Vector2.right;
                RigidBody.gravityScale = 1;
                break;
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.collider.gameObject))
        {
            Destroy(gameObject);
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
                if (!HasCollidedWithAPlatformAtDetectAngle())
                {
                    FlipMoveDirectionHorizontal();
                }
                break;
            case EnemyTypes.AirVertical:
                if (Game_Manager.CheckIfEnoughTimeHasPassed(_timeSinceLastMoveDirFlip, _timeUntilMoveDirFlip))
                {
                    _timeSinceLastMoveDirFlip = Time.time;
                    FlipMoveDirectionVertical();
                }
                break;
            case EnemyTypes.AirHorizontal:
                if (Game_Manager.CheckIfEnoughTimeHasPassed(_timeSinceLastMoveDirFlip, _timeUntilMoveDirFlip))
                {
                    _timeSinceLastMoveDirFlip = Time.time;
                    FlipMoveDirectionHorizontal();
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
    private void FlipMoveDirectionHorizontal()
    {
        DetectVector = Vector2.Reflect(DetectVector, Vector2.right);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        _moveDirection *= -1;
    }
    private void FlipMoveDirectionVertical()
    {
        DetectVector = Vector2.Reflect(DetectVector, Vector2.up);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        _moveDirection *= -1;
    }
}
