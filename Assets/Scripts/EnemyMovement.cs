using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [SerializeField]
    private EnemyTypes _enemyMoveType;
    [SerializeField]
    [Tooltip("For airborne enemies, a point to travel to when moving")]
    private Transform _patrolPointStart, _patrolPointEnd;
    private Transform _patrolPointTarget;

    private enum EnemyTypes : int
    {
        Ground, AirVertical, AirHorizontal
    }

    private new void Awake()
    {
        base.Awake();

        // Set physics values based on the enemy type
        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 1;
                break;
            case EnemyTypes.AirVertical:
                MoveDirection = Vector2.up;
                RigidBody.gravityScale = 0;
                // Only allow changes to y position;
                RigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
                RigidBody.freezeRotation = true;
                SetInitialPositionToPatrolPoint();
                break;
            case EnemyTypes.AirHorizontal:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 0;
                // Only allow changes to x position;
                RigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
                RigidBody.freezeRotation = true;
                SetInitialPositionToPatrolPoint();
                break;
            default:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 1;
                break;
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            Destroy(gameObject);
        }
    }
    private void SetInitialPositionToPatrolPoint()
    {
        if (_patrolPointStart)
        {
            // speed = distance / time
            float distance = 1.0f * MoveForce * EffectMoveFlip.TimeCooldownEffect;
            Debug.Log($"{name}: I am at {_patrolPointStart.transform.position.x}, {_patrolPointStart.transform.position.y} and expected distance to end point is {distance}");
        }

        if (_patrolPointStart == _patrolPointEnd)
            throw new Exception("The patrol points are the same, enemy can't move");
        transform.position = _patrolPointStart.position;
        _patrolPointTarget = _patrolPointEnd;
    }
    protected override void DoMovement()
    {
        // Check conditions to flip move direction, based on the enemy type
        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
                // Did not detect a platform in front of it
                if (!HasCollidedWithPlatformAtDetectAngle())
                {
                    FlipMoveDirectionHorizontal();
                }
                break;
            //case EnemyTypes.AirVertical:
            //case EnemyTypes.AirHorizontal:
            //    Vector2 distanceToTargetTransform = _patrolPointTarget.position - transform.position;
            //    RigidBody.velocity = MoveForce * distanceToTargetTransform;
            //    if (Vector2.Distance(transform.position, _patrolPointTarget.position) <= 0.1f)
            //    {
            //        if (_patrolPointTarget == _patrolPointEnd)
            //            _patrolPointTarget = _patrolPointStart;
            //        else
            //            _patrolPointTarget = _patrolPointEnd;
            //    }
            //    break;
            case EnemyTypes.AirVertical:
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
                if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
                {
                    EffectMoveFlip.SetNewTimeEffectApply();
                    FlipMoveDirectionVertical();
                }
                break;
            case EnemyTypes.AirHorizontal:
                RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
                if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
                {
                    EffectMoveFlip.SetNewTimeEffectApply();
                    FlipMoveDirectionHorizontal();
                }
                break;
            default:
                break;
        }
    }
    private void FlipMoveDirectionHorizontal()
    {
        FlipMoveDirection(Vector2.right);
    }
    private void FlipMoveDirectionVertical()
    {
        FlipMoveDirection(Vector2.up);
    }
    /// <summary>
    /// Should only be called by "FlipMoveDirection..." functions
    /// </summary>
    private void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
        Debug.Log($"{name}: Flipping movement, end point at {transform.position.x}, {transform.position.y}");
    }
}
