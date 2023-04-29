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
    private Transform _patrolPointTarget, _patrolPointCurrent;

    private enum EnemyTypes : int
    {
        Ground, AirVertical, AirHorizontal
    }
    private enum EnemyMoveTypes : int
    {
        Timed, Patrol
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
        if (!_patrolPointStart)
            throw new Exception("The start patrol point is not set");
        if (_patrolPointStart == _patrolPointEnd)
            throw new Exception("The patrol points are the same, enemy can't move");

        _patrolPointCurrent = _patrolPointStart;
        transform.position = _patrolPointCurrent.position;
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
                    FlipMoveDirection(Vector2.right);
                break;
            case EnemyTypes.AirVertical:
            case EnemyTypes.AirHorizontal:
                if (Game_Manager.ShouldAirborneEnemiesPatrol)
                    DoPatrolPointMovement();
                else
                    DoTimedVelocityMovement();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Air enemies move via "patrol points" in the scene
    /// </summary>
    private void DoPatrolPointMovement()
    {
        Vector2 distanceToTargetTransform = _patrolPointTarget.position - transform.position;
        RigidBody.velocity = MoveForce * distanceToTargetTransform.normalized;
        if (Vector2.Distance(transform.position, _patrolPointTarget.position) <= 0.1f && Vector2.Distance(transform.position, _patrolPointCurrent.position) > 0.1f)
        {
            if (_patrolPointTarget == _patrolPointEnd)
            {
                _patrolPointCurrent = _patrolPointEnd;
                _patrolPointTarget = _patrolPointStart;
            }
            else
            {
                _patrolPointCurrent = _patrolPointStart;
                _patrolPointTarget = _patrolPointEnd;
            }
        }
    }
    /// <summary>
    /// Air enemies move via their set move force and time until switching directions
    /// </summary>
    private void DoTimedVelocityMovement()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
        if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
        {
            EffectMoveFlip.SetNewTimeEffectApply();
            if (_enemyMoveType == EnemyTypes.AirHorizontal)
                FlipMoveDirection(Vector2.right);
            else if (_enemyMoveType == EnemyTypes.AirVertical)
                FlipMoveDirection(Vector2.up);
        }
    }
    private void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
    }
}
